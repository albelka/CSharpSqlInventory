using Nancy;
using System.Collections.Generic;
using System;
using Inventory.Objects;

namespace Inventory
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {

        return View["index.cshtml"];
      };
      Post["/"] = _ => {
        Rock newRock = new Rock(Request.Form["name"], Request.Form["mass"]);
        newRock.Save();
        List<Rock> allRocks = Rock.GetAll();

        return View["index.cshtml", allRocks];
      };

    }
  }
}
