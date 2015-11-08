/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="sharpbox.Web.Environment.ts"/>
/// <reference path="sharpbox.Web.Form.ts"/>
/// <reference path="Template/SharpCrudTemplate.ts"/>

//SEE: http://stackoverflow.com/a/2880929
var urlParams;
(window.onpopstate = function () {
    var match,
        pl = /\+/g,  // Regex for replacing addition symbol with a space
        search = /([^&=]+)=?([^&]*)/g,
        decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
        query = window.location.search.substring(1);

    urlParams = {};
    while (match = search.exec(query))
        urlParams[decode(match[1])] = decode(match[2]);
})();

var url = window.location.pathname.split("/");
var container = "#example";
var instanceName = url[1] || "Home";
var sharpId = urlParams["sharpId"];
var site = new sharpbox.Web.Site();

site.loadEnvironmentById("1be0590f-e0db-4923-988a-384d473dfb4d", () => {
    $("#appName").html(site.environment.ApplicationName);
    loadEditForm(container, instanceName, sharpId);
});

var loadEditForm = (containerSelector: string, domainName: string, id: string) => {
    var detail = new sharpbox.Web.ViewModel<any>(domainName);
    detail.getSchema(() => {
        var htmlStrategy: sharpbox.Web.IHtmlStrategy = new sharpbox.Web.BootstrapHtmlStrategy();
        var formName = "UpdateForm";
        detail.form = new sharpbox.Web.Form(detail.schema, formName, detail.controllerUrl, "Update", "POST", htmlStrategy);
        var template = sharpbox.Web.Templating.editForm(detail);
        $(containerSelector).html(template);
        detail.form.htmlStrategy.wireSubmit(formName);

        if (id != null) {
            detail.getById(id, () => {
                detail.form.bindToForm(detail.instance);
            });
        }
    });       
}

var exampleOfUsingStrongyTyped = () : void => {
    var test = new sharpbox.Web.ViewModel<sharpbox.App.Model.Environment>("Environment");

    // Use a lambda to use the loaded schema to init the form.
    test.getSchema(() => {
        var htmlStrategy: sharpbox.Web.IHtmlStrategy = new sharpbox.Web.BootstrapHtmlStrategy();
        var formName = "UpdateForm";
        test.form = new sharpbox.Web.Form(test.schema, formName, test.controllerUrl, "Update", "POST", htmlStrategy);
        var template = sharpbox.Web.Templating.editForm(test);
        $(container).html(template);
        test.form.htmlStrategy.wireSubmit(formName);
        test.form.bindToForm(site.environment);
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

    var wat: number = test.instance.BrandTypeId;

//test.execute(sharpbox.Domain.TestController.Command.TestCommand);
};
