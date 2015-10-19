

 
 

 


declare module sharpbox.Audit.Dispatch {
	interface AuditCommands {
	}
	interface AuditEvents extends sharpbox.Common.Dispatch.Model.EventName {
	}
}
declare module sharpbox.Common.Dispatch.Model {
	interface EventName extends sharpbox.Common.Type.EnumPattern {
		EventNameId: number;
		Name: string;
		ApplicationId: System.Guid;
	}
	interface CommandName extends sharpbox.Common.Type.EnumPattern {
		CommandNameId: number;
		Name: string;
		ApplicationId: System.Guid;
	}
}
declare module sharpbox.Common.Type {
	interface EnumPattern {
	}
}
declare module System {
	interface Guid {
	}
	
    interface Type {}

	interface Delegate {
		Target: any;
	}
}
declare module sharpbox.Dispatch.Model {
	export interface Request extends sharpbox.Dispatch.Model.BasePackage {
		RequestId: number;
		RequestUniqueKey: System.Guid;
		Message: string;
		CommandNameId: number;
		CommandName: sharpbox.Common.Dispatch.Model.CommandName;
		Action: System.Delegate;
		CreatedDate: Date;
		ApplicationId: System.Guid;
	}
	interface BasePackage {
		Entity: any;
		Type: System.Type;
		ResponseType: sharpbox.Dispatch.Model.ResponseTypes;
		SerializedEntity: string;
		SerializeEntityType: string;
	}
	interface Response extends sharpbox.Dispatch.Model.BasePackage {
		ResponseId: number;
		ResponseUniqueKey: System.Guid;
		Message: string;
		EventNameId: number;
		EventName: sharpbox.Common.Dispatch.Model.EventName;
		RequestId: number;
		RequestUniqueKey: System.Guid;
		Request: sharpbox.Dispatch.Model.Request;
		UserId: string;
		CreatedDate: Date;
		ApplicationId: System.Guid;
	}
}

declare module sharpbox.Email.Dispatch {
	interface EmailCommands {
	}
	interface EmailEvents {
	}
}
declare module sharpbox.Io.Dispatch {
	interface IoCommands {
	}
	interface IoEvents {
	}
}
declare module sharpbox.Io.Model {
	export interface FileDetail {
		FilePath: string;
		Data: number[];
	}
}
declare module sharpbox.Membership.Model {
	export interface UserRole extends sharpbox.Common.Type.EnumPattern {
		UserRoleNameId: number;
		Name: string;
	}
}
declare module sharpbox.Notification.Dispatch {
	interface NotificationCommands {
	}
	interface NotificationEvents {
	}
}
declare module sharpbox.Notification.Model {
	export interface BackLogItem {
		BackLogItemId: number;
		BackLogItemUniqueId: System.Guid;
		RequestId: number;
		RequestUniqueKey: System.Guid;
		ResponseId: number;
		ResponseUniqueKey: System.Guid;
		UserId: string;
		WasSent: boolean;
		SentDate: Date;
		AttempNumber: number;
		AttemptMessage: string;
		PreviousAttempTime: Date;
		NextAttempTime: Date;
		To: string[];
		From: string;
		Subject: string;
		Message: string;
		ApplicationId: System.Guid;
	}
	interface Subscriber {
		SubscriberId: number;
		EventName: sharpbox.Common.Dispatch.Model.EventName;
		Type: System.Type;
		UserId: string;
		ApplicationId: System.Guid;
		SerializeEntityType: string;
	}
}

declare module sharpbox.Dispatch.Model {
	export enum ResponseTypes {
		Info = 0,
		Warning = 1,
		Success = 2,
		Error = 3
	}
}