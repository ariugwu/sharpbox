/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.Web {
    export class PageArgs {
        url: string[];
        controllerName: string;
        actionName: string;
        id: string;

        constructor(urlParams: any) {
            this.url = window.location.pathname.split("/");
            this.controllerName = this.url[1] || "Environment";
            this.actionName = this.url[2];
            this.id = urlParams["id"];
        }

    }
}

