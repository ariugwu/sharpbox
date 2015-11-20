/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="sharpbox.Web.Environment.ts"/>
/// <reference path="sharpbox.Web.Form.ts"/>
/// <reference path="Template/SharpCrudTemplate.ts"/>
/// <reference path="sharpbox.Web.Scaffold.ts"/>
//SEE: http://stackoverflow.com/a/2880929
var urlParams;
(window.onpopstate = function () {
    var match, pl = /\+/g, // Regex for replacing addition symbol with a space
    search = /([^&=]+)=?([^&]*)/g, decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); }, query = window.location.search.substring(1);
    urlParams = {};
    while (match = search.exec(query))
        urlParams[decode(match[1])] = decode(match[2]);
})();
$(document).ready(function () {
    var url = window.location.pathname.split("/");
    var container = "#example";
    var instanceName = url[1] || "Environment";
    var actionName = url[2];
    var id = urlParams["id"];
    var site = new sharpbox.Web.Site();
    var scaffold = new sharpbox.Web.Scaffold();
    console.log(id);
    site.loadEnvironmentById(1, function () {
        $("#appName").html(site.environment.ApplicationName);
        if (id != null || actionName == "Detail") {
            scaffold.loadEditForm(container, instanceName, id); //SEE sharpbox.Web.Scaffold.ts
        }
        else {
            scaffold.loadGrid(container, instanceName);
        }
    });
});
//var exampleOfUsingStrongyTyped = (): void => {
//    var test = new sharpbox.Web.ViewModel<sharpbox.App.Model.Environment>("Environment");
//    // Use a lambda to use the loaded schema to init the form.
//    test.getSchema(() => {
//        var htmlStrategy: sharpbox.Web.IHtmlStrategy = new sharpbox.Web.BootstrapHtmlStrategy();
//        var formName = "UpdateForm";
//        test.form = new sharpbox.Web.Form(test.schema, formName, test.controllerUrl, "Update", "POST", htmlStrategy);
//        var template = sharpbox.Web.Templating.editForm(test);
//        $(container).html(template);
//        test.form.htmlStrategy.wireSubmit(formName);
//        test.form.bindToForm(site.environment);
//    });
//    test.instance = {
//        EnvironmentId: 0,
//        ApplicationName: "Sample Application",
//        BaseUrl: "http://example.com",
//        CacheKey: "any",
//        UploadDirectory: "~/Uploads/",
//        LogoLocation: "~/Images/Logo",
//        BrandTypeId: 1,
//        BrandType: null,
//        EnvironmentTypeId: 1,
//        EnvironmentType: null,
//        TechSheetId: 1,
//        TechSheet: null,
//        SharpId: null
//    };
//    var wat: number = test.instance.BrandTypeId;
//    //test.execute(sharpbox.Domain.TestController.Command.TestCommand);
//};
//# sourceMappingURL=sharpbox.js.map