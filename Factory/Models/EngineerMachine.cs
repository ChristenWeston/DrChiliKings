using System.Collections.Generic;

namespace Factory.Models
{
  public class EngineerMachine
  {
    public int EngineerMachineId { get; set; }
    public int EngineerId { get; set; }
    public int MachineId { get; set; }
    public virtual Engineer engineer { get; set; }
    public virtual Machine machine { get; set; }
  }
}