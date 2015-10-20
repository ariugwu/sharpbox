using System.Linq;
using System.Text;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.WebLibrary.Helpers.TypeScript
{   
    public class DomainMetadata
    {
      public string Generate()
      {
          var ts = new StringBuilder();

          var classes = MetaLoader.GetAllMetaDataClasses();

          // Let's make our interfaces but don't include any of our sharpbox stuff
          foreach (var c in classes.Where(x => !x.Name.Contains("Sharpbox") && !x.Name.Contains("IDispatch")))
          {
              ts.AppendLine(string.Format("module sharpbox.Domain.{0} {{", c.Name));

              var commandNames = c.GetFields().Where(x => x.FieldType == typeof(CommandName));
              var eventNames = c.GetFields().Where(x => x.FieldType == typeof(EventName));
              var uiActionNames = c.GetFields().Where(x => x.FieldType == typeof(UiAction));
              var routineNames = c.GetFields().Where(x => x.FieldType == typeof(RoutineName));

              // Write out the Command(s)
              ts.AppendLine(string.Format("export class {0} {{", "Command"));

              ts.AppendLine("constructor() {throw new Error(\"Cannot new this class\");}");

              foreach (var cmd in commandNames)
              {
                  ts.AppendLine(string.Format("static {0} = \"{0}\";",cmd.Name));
              }

              ts.AppendLine("}");

              ts.AppendLine(string.Empty);

              // Write out the Event(s)

              ts.AppendLine(string.Format("export class {0} {{", "Event"));
              ts.AppendLine("constructor() {throw new Error(\"Cannot new this class\");}");

              foreach (var cmd in eventNames)
              {
                  ts.AppendLine(string.Format("static {0} = \"{0}\";", cmd.Name));
              }

              ts.AppendLine("}");

              ts.AppendLine(string.Empty);

              // Write out the Ui Action(s)
              ts.AppendLine(string.Format("export class {0} {{", "UiAction"));
              ts.AppendLine("constructor() {throw new Error(\"Cannot new this class\");}");

              foreach (var cmd in uiActionNames)
              {
                  ts.AppendLine(string.Format("static {0} = \"{0}\";", cmd.Name));
              }

              ts.AppendLine("}");

              ts.AppendLine(string.Empty);

              // Write out the Routine(s)
              ts.AppendLine(string.Format("export class {0} {{", "Routine"));
              ts.AppendLine("constructor() {throw new Error(\"Cannot new this class\");}");

              foreach (var cmd in routineNames)
              {
                  ts.AppendLine(string.Format("static {0} = \"{0}\";", cmd.Name));
              }

              ts.AppendLine("}");

              ts.AppendLine(string.Empty);

              ts.AppendLine("}"); // close out module.

          }

          return ts.ToString();
      }
    }
}
