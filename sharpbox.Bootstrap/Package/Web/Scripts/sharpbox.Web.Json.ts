/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="Typings/collections.d.ts"/>

/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.Web.Json {
    export interface ISchema {
        type: string;
        format: SchemaType;
    }

    export enum SchemaType {
        Array,
        Boolean,
        Integer,
        Number,
        Null,
        Object,
        String
    }
}