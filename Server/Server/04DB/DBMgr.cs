﻿using MySql.Data.MySqlClient;
using Protocol;
using System;
using System.Collections.Generic;

public class DBMgr
{
    private static DBMgr _instance;
    private static MySqlConnection conn;

    public static DBMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DBMgr();
            }
            return _instance;
        }
    }

    public void Init()
    {
        conn = new MySqlConnection("server = localhost;User Id = root;password = ;Database = darkgod;Charset = utf8");
        conn.Open();
        Common.Log("DBMgr Init Done.");
    }

    public PlayerData QueryPlayerData(string acct, string pass)
    {
        bool isNew = true;
        PlayerData playerData = null;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                isNew = false;
                string _pass = reader.GetString("pass");
                if (_pass.Equals(pass))
                {
                    playerData = new PlayerData
                    {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("level"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond"),
                        crystal = reader.GetInt32("crystal"),
                        hp = reader.GetInt32("hp"),
                        ad = reader.GetInt32("ad"),
                        ap = reader.GetInt32("ap"),
                        addef = reader.GetInt32("addef"),
                        apdef = reader.GetInt32("apdef"),
                        dodge = reader.GetInt32("dodge"),
                        pierce = reader.GetInt32("pierce"),
                        critical = reader.GetInt32("critical"),
                        guideid = reader.GetInt32("guideid"),
                        time = reader.GetInt64("time"),
                        fuben = reader.GetInt32("fuben")
                    };

                    //数据示意 1#2#2#4#3#7
                    string[] strongStrArr = reader.GetString("strong").Split('#');
                    int[] _strongArr = new int[6];
                    for (int i = 0; i < strongStrArr.Length; i++)
                    {
                        if (strongStrArr[i] == "")
                        {
                            continue;
                        }
                        if (int.TryParse(strongStrArr[i], out int starLv))
                        {
                            _strongArr[i] = starLv;
                        }
                        else
                        {
                            Common.Log("Parse Strong Data Error!", LogType.Error); 
                        }
                    }
                    playerData.strongArr = _strongArr;

                    //数据示意：1|0|0#2|0|0#3
                    string[] taskStrArr = reader.GetString("task").Split('#');
                    List<string> tempTaskStrArr = new List<string>();
                    for (int i = 0; i < taskStrArr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(taskStrArr[i]))
                        {
                            continue;
                        }
                        else if (taskStrArr[i].Length >= 5)
                        {
                            tempTaskStrArr.Add(taskStrArr[i]);
                        }
                        else
                        {
                            throw new Exception("task array data error");
                        }
                    }
                    playerData.taskArr = tempTaskStrArr.ToArray();
                }
            }
        }
        catch (System.Exception e)
        {
            Common.Log("Query PlayerData By Acct&Pass Error:" + e, LogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (isNew)
            {
                playerData = new PlayerData
                {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 5000,
                    diamond = 500,
                    crystal = 500,
                    hp = 2000,
                    ad = 275,
                    ap = 265,
                    addef = 67,
                    apdef = 43,
                    dodge = 7,
                    pierce = 5,
                    critical = 2,
                    guideid = 1001,
                    strongArr = new int[6],
                    time = TimerSvc.Instance.GetNowTime(),
                    taskArr = new string[6],
                    fuben = 10001
                };
                for (int i = 0; i < playerData.taskArr.Length; i++)
                {
                    playerData.taskArr[i] = (i + 1) + "|0|0";
                }

                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }
        return playerData;
    }

    private int InsertNewAcctData(string acct, string pass, PlayerData pd)
    {
        int id = -1;
        try
        {
            MySqlCommand cmd = new MySqlCommand("insert into account set acct=@acct,pass =@pass,name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal," +
                "hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid=@guideid,strong=@strong,time=@time,task=@task,fuben=@fuben", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("level", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            cmd.Parameters.AddWithValue("crystal", pd.diamond);
            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);
            cmd.Parameters.AddWithValue("guideid", pd.guideid);
            cmd.Parameters.AddWithValue("time", pd.time);

            string strongInfo = string.Join("#", pd.strongArr);
            cmd.Parameters.AddWithValue("strong", strongInfo);

            string taskInfo = string.Join("#", pd.taskArr);
            cmd.Parameters.AddWithValue("task", taskInfo);
            cmd.Parameters.AddWithValue("fuben", pd.fuben);

            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (System.Exception e)
        {
            Common.Log("Insert PlayerData Error:" + e, LogType.Error);
        }
        return id;
    }

    public bool QueryNameData(string name)
    {
        bool exist = false;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where name = @name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                exist = true;
            }
        }
        catch (System.Exception e)
        {
            Common.Log("Query Name State Error:" + e, LogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }

        return exist;
    }

    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand("update account set name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal," +
               "hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid=@guideid,strong=@strong,task=@task,fuben=@fuben" + " where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", playerData.name);
            cmd.Parameters.AddWithValue("level", playerData.lv);
            cmd.Parameters.AddWithValue("exp", playerData.exp);
            cmd.Parameters.AddWithValue("power", playerData.power);
            cmd.Parameters.AddWithValue("coin", playerData.coin);
            cmd.Parameters.AddWithValue("diamond", playerData.diamond);
            cmd.Parameters.AddWithValue("crystal", playerData.crystal);
            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("ap", playerData.ap);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);
            cmd.Parameters.AddWithValue("guideid", playerData.guideid);
            cmd.Parameters.AddWithValue("time", playerData.time);

            string strongInfo = string.Join("#", playerData.strongArr);
            cmd.Parameters.AddWithValue("strong", strongInfo);

            string taskInfo = string.Join("#", playerData.taskArr);
            cmd.Parameters.AddWithValue("task", taskInfo);
            cmd.Parameters.AddWithValue("fuben", playerData.fuben);

            cmd.ExecuteNonQuery();
        }
        catch (System.Exception e)
        {
            Common.Log("Update PlayerData Error:" + e, LogType.Error);
            return false;
        }
        return true;
    }

}