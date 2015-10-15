using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.Util.TypeScript
{
  /// <summary>
  /// Used to decorate a class which contains the Commands, Events, Routines, and UiActions for a domain.
  /// Used in cases where generation of meta data should be type safe.
  /// All static fields within this class which match the command/event/routine/uiAction types will be converted to TS enums.
  /// The TS enum will have the name of the T class "MetadataEnums". For example "UserMetadataEnums.ts"
  /// </summary>
  /// <typeparam name="T">The constraint uses to identify one metadata set from another.</typeparam>
  public interface ISharpboxMetadata<T>
  {
  }
}
