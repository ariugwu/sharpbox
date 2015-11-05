/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="Typings/collections.d.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var Json;
        (function (Json) {
            (function (SchemaType) {
                SchemaType[SchemaType["Array"] = 0] = "Array";
                SchemaType[SchemaType["Boolean"] = 1] = "Boolean";
                SchemaType[SchemaType["Integer"] = 2] = "Integer";
                SchemaType[SchemaType["Number"] = 3] = "Number";
                SchemaType[SchemaType["Null"] = 4] = "Null";
                SchemaType[SchemaType["Object"] = 5] = "Object";
                SchemaType[SchemaType["String"] = 6] = "String";
            })(Json.SchemaType || (Json.SchemaType = {}));
            var SchemaType = Json.SchemaType;
        })(Json = Web.Json || (Web.Json = {}));
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
//# sourceMappingURL=sharpbox.Web.Json.js.map