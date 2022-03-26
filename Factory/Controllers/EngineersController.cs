using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class EngineersController : Controller
  {
    private readonly FactoryContext _db;

    public EngineersController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {  
      ViewBag.PageTitle = "Engineer Index";
      List<Engineer> thisEngineer = _db.Engineers
        .Include(engineer => engineer.JoinEntities)
        .ThenInclude(join => join.machine)
        .ToList();
      return View(thisEngineer);
    }

    public ActionResult Create()
    {
      ViewBag.PageTitle = "Add Engineer";
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Engineer engineer, int MachineId)
    {
      ViewBag.PageTitle = "Add Engineer";
      _db.Engineers.Add(engineer);
      _db.SaveChanges();
      if (MachineId != 0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() { MachineId = MachineId, EngineerId = engineer.EngineerId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      ViewBag.PageTitle = "Engineer Details";
      var thisEngineer = _db.Engineers
          .Include(engineer => engineer.JoinEntities)
          .ThenInclude(join => join.machine)
          .FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    } 

    public ActionResult Edit(int id)
    {
      ViewBag.PageTitle = "Edit Engineer";
      var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult Edit(Engineer engineer)
    {
      ViewBag.PageTitle = "Edit Engineer";
      _db.Entry(engineer).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      ViewBag.PageTitle = "Delete Engineer";
      var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      return View(thisEngineer);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      ViewBag.PageTitle = "Delete Engineer";
      var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
      _db.Engineers.Remove(thisEngineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddMachine(int id)
      {
        ViewBag.PageTitle = "Add Machine to Engineer";
        ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Name");
        var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
        return View(thisEngineer);
      }

    [HttpPost]
    public ActionResult AddMachine(Engineer engineer, int MachineId)
    {
      ViewBag.PageTitle = "Add Machine to Engineer";
      if (MachineId != 0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() { MachineId = MachineId, EngineerId = engineer.EngineerId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }
    
    [HttpPost]
    public ActionResult DeleteMachine(int joinId)
    {
      ViewBag.PageTitle = "Delete Machine from Engineer";
      var joinEntry = _db.EngineerMachines.FirstOrDefault(entry => entry.EngineerMachineId == joinId);
      _db.EngineerMachines.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }  
}    