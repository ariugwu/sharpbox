/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="sharpbox.Web.Form.ts"/>
/// <reference path="Template/SharpCrudTemplate.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var test = new sharpbox.Web.ViewModel("Environment");
//while (test.ajaxPause) {
//  console.log("waiting..");
//}
// Use a lambda to use the loaded schema to init the form.
test.getSchema(function () {
    test.form = new sharpbox.Web.Form(test.schema, "UpdateForm", test.controllerUrl, "Update", "POST");
    var template = sharpbox.Web.Templating.editForm(test);
    $("#example").html(template);
});
test.instance = {
    EnvironmentId: 0,
    ApplicationName: "Sample Application",
    BaseUrl: "http://example.com",
    CacheKey: "any",
    UploadDirectory: "~/Uploads/",
    LogoLocation: "~/Images/Logo",
    BrandTypeId: 1,
    BrandType: null,
    EnvironmentTypeId: 1,
    EnvironmentType: null,
    TechSheetId: 1,
    TechSheet: null,
    SharpId: null
};
var wat = test.instance.BrandTypeId;
//test.execute(sharpbox.Domain.TestController.Command.TestCommand); 
