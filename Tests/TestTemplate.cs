using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Inventory.Objects;

namespace  Inventory
{
  public class RockTest : IDisposable
  {
    public RockTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Rock.DeleteAll();
    }
    [Fact]
    public void GetAll_DatabaseEmpty_True()
    {
      //Arrange, Act
      int result = Rock.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void EqualsOverride_EqualObjects_True()
    {
      Rock newRock = new Rock("quartz", 3);
      Rock otherRock = new Rock("quartz", 3);

      Assert.Equal(newRock, otherRock);
    }
    [Fact]
    public void Save_SaveObjectToDB_True()
    {
      Rock newRock = new Rock("quartz", 3);
      List<Rock> expectedResult = new List<Rock>{newRock};

      newRock.Save();
      List<Rock> result = Rock.GetAll();

      Assert.Equal(expectedResult, result);
    }
    [Fact]
    public void Test_Find_FindsRockInDatabase()
    {
      Rock newRock = new Rock("geode", 6);
      newRock.Save();

      Rock foundRock = Rock.Find(newRock.GetId());

      Assert.Equal(newRock, foundRock);
    }
    [Fact]
    public void Test_DeleteAll_ClearsDatabase()
    {
      Rock newRock = new Rock("sapphire", 1);
      newRock.Save();

      Rock.DeleteAll();
      int result = Rock.GetAll().Count;

      Assert.Equal(0, result);
    }
  }
}
