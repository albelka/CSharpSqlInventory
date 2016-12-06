using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inventory.Objects
{
  public class Rock
  {
    string _name;
    int _mass;
    int _id;

    public Rock(string name, int mass,int id = 0)
    {
      _id = id;
      _name = name;
      _mass = mass;
    }

    public string GetName()
    {
      return _name;
    }
    public int GetMass()
    {
      return _mass;
    }
    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherRock)
    {
      if (!(otherRock is Rock))
      {
        return false;
      }
      else
      {
        Rock newRock = (Rock) otherRock;
        bool nameEqual = (this.GetName() == newRock.GetName());
        bool massEqual = (this.GetMass() == newRock.GetMass());
        bool idEqual = (this.GetId() == newRock.GetId());
        return (nameEqual && massEqual && idEqual);
      }
    }


    public static List<Rock> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      List<Rock> allRocks = new List<Rock>{};

      SqlCommand cmd = new SqlCommand("SELECT * FROM rocks;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int mass = rdr.GetInt32(2);
        Rock newRock = new Rock(name, mass, id);
        allRocks.Add(newRock);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allRocks;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO rocks (name, mass) OUTPUT INSERTED.id VALUES (@rockName, @rockMass)", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@rockName";
      nameParam.Value = this.GetName();
      SqlParameter massParam = new SqlParameter();
      massParam.ParameterName = "@rockMass";
      massParam.Value = this.GetMass();
      cmd.Parameters.Add(nameParam);
      cmd.Parameters.Add(massParam);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM rocks;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Rock Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM rocks WHERE id = @RockId;", conn);
      SqlParameter rockIdParameter = new SqlParameter();
      rockIdParameter.ParameterName = "@RockId";
      rockIdParameter.Value = id.ToString();
      cmd.Parameters.Add(rockIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundRockId = 0;
      string foundRockName = "";
      int foundRockMass = 0;

      while(rdr.Read())
      {
        foundRockId = rdr.GetInt32(0);
        foundRockName = rdr.GetString(1);
        foundRockMass = rdr.GetInt32(2);
      }

      Rock foundRock = new Rock( foundRockName, foundRockMass, foundRockId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundRock;
    }

  }
}
