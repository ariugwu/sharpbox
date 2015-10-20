

 
 

 


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
	interface Type extends System.Reflection.MemberInfo {
		MemberType: System.Reflection.MemberTypes;
		DeclaringType: System.Type;
		DeclaringMethod: System.Reflection.MethodBase;
		ReflectedType: System.Type;
		StructLayoutAttribute: System.Runtime.InteropServices.StructLayoutAttribute;
		GUID: System.Guid;
		DefaultBinder: System.Reflection.Binder;
		Module: System.Reflection.Module;
		Assembly: System.Reflection.Assembly;
		TypeHandle: System.RuntimeTypeHandle;
		FullName: string;
		Namespace: string;
		AssemblyQualifiedName: string;
		BaseType: System.Type;
		TypeInitializer: System.Reflection.ConstructorInfo;
		IsNested: boolean;
		Attributes: System.Reflection.TypeAttributes;
		GenericParameterAttributes: System.Reflection.GenericParameterAttributes;
		IsVisible: boolean;
		IsNotPublic: boolean;
		IsPublic: boolean;
		IsNestedPublic: boolean;
		IsNestedPrivate: boolean;
		IsNestedFamily: boolean;
		IsNestedAssembly: boolean;
		IsNestedFamANDAssem: boolean;
		IsNestedFamORAssem: boolean;
		IsAutoLayout: boolean;
		IsLayoutSequential: boolean;
		IsExplicitLayout: boolean;
		IsClass: boolean;
		IsInterface: boolean;
		IsValueType: boolean;
		IsAbstract: boolean;
		IsSealed: boolean;
		IsEnum: boolean;
		IsSpecialName: boolean;
		IsImport: boolean;
		IsSerializable: boolean;
		IsAnsiClass: boolean;
		IsUnicodeClass: boolean;
		IsAutoClass: boolean;
		IsArray: boolean;
		IsGenericType: boolean;
		IsGenericTypeDefinition: boolean;
		IsConstructedGenericType: boolean;
		IsGenericParameter: boolean;
		GenericParameterPosition: number;
		ContainsGenericParameters: boolean;
		IsByRef: boolean;
		IsPointer: boolean;
		IsPrimitive: boolean;
		IsCOMObject: boolean;
		HasElementType: boolean;
		IsContextful: boolean;
		IsMarshalByRef: boolean;
		GenericTypeArguments: System.Type[];
		IsSecurityCritical: boolean;
		IsSecuritySafeCritical: boolean;
		IsSecurityTransparent: boolean;
		UnderlyingSystemType: System.Type;
	}
	interface RuntimeMethodHandle {
		Value: number;
	}
	interface RuntimeFieldHandle {
		Value: number;
	}
	interface ModuleHandle {
		MDStreamVersion: number;
	}
	interface Attribute {
		TypeId: any;
	}
	interface RuntimeTypeHandle {
		Value: number;
	}
	interface Delegate {
		Method: System.Reflection.MethodInfo;
		Target: any;
	}
	interface Exception {
		Message: string;
		Data: any;
		InnerException: System.Exception;
		TargetSite: System.Reflection.MethodBase;
		StackTrace: string;
		HelpLink: string;
		Source: string;
		HResult: number;
	}
}
declare module sharpbox.Dispatch.Model {
	interface Request extends sharpbox.Dispatch.Model.BasePackage {
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
declare module System.Reflection {
	interface MemberInfo {
		MemberType: System.Reflection.MemberTypes;
		Name: string;
		DeclaringType: System.Type;
		ReflectedType: System.Type;
		CustomAttributes: System.Reflection.CustomAttributeData[];
		MetadataToken: number;
		Module: System.Reflection.Module;
	}
	interface CustomAttributeData {
		AttributeType: System.Type;
		Constructor: System.Reflection.ConstructorInfo;
		ConstructorArguments: System.Reflection.CustomAttributeTypedArgument[];
		NamedArguments: System.Reflection.CustomAttributeNamedArgument[];
	}
	interface ConstructorInfo extends System.Reflection.MethodBase {
		MemberType: System.Reflection.MemberTypes;
	}
	interface MethodBase extends System.Reflection.MemberInfo {
		MethodImplementationFlags: System.Reflection.MethodImplAttributes;
		MethodHandle: System.RuntimeMethodHandle;
		Attributes: System.Reflection.MethodAttributes;
		CallingConvention: System.Reflection.CallingConventions;
		IsGenericMethodDefinition: boolean;
		ContainsGenericParameters: boolean;
		IsGenericMethod: boolean;
		IsSecurityCritical: boolean;
		IsSecuritySafeCritical: boolean;
		IsSecurityTransparent: boolean;
		IsPublic: boolean;
		IsPrivate: boolean;
		IsFamily: boolean;
		IsAssembly: boolean;
		IsFamilyAndAssembly: boolean;
		IsFamilyOrAssembly: boolean;
		IsStatic: boolean;
		IsFinal: boolean;
		IsVirtual: boolean;
		IsHideBySig: boolean;
		IsAbstract: boolean;
		IsSpecialName: boolean;
		IsConstructor: boolean;
	}
	interface CustomAttributeTypedArgument {
		ArgumentType: System.Type;
		Value: any;
	}
	interface CustomAttributeNamedArgument {
		MemberInfo: System.Reflection.MemberInfo;
		TypedValue: System.Reflection.CustomAttributeTypedArgument;
		MemberName: string;
		IsField: boolean;
	}
	interface Module {
		CustomAttributes: System.Reflection.CustomAttributeData[];
		MDStreamVersion: number;
		FullyQualifiedName: string;
		ModuleVersionId: System.Guid;
		MetadataToken: number;
		ScopeName: string;
		Name: string;
		Assembly: System.Reflection.Assembly;
		ModuleHandle: System.ModuleHandle;
	}
	interface Assembly {
		CodeBase: string;
		EscapedCodeBase: string;
		FullName: string;
		EntryPoint: System.Reflection.MethodInfo;
		ExportedTypes: System.Type[];
		DefinedTypes: System.Reflection.TypeInfo[];
		Evidence: any;
		PermissionSet: any;
		IsFullyTrusted: boolean;
		SecurityRuleSet: System.Security.SecurityRuleSet;
		ManifestModule: System.Reflection.Module;
		CustomAttributes: System.Reflection.CustomAttributeData[];
		ReflectionOnly: boolean;
		Modules: System.Reflection.Module[];
		Location: string;
		ImageRuntimeVersion: string;
		GlobalAssemblyCache: boolean;
		HostContext: number;
		IsDynamic: boolean;
	}
	interface MethodInfo extends System.Reflection.MethodBase {
		MemberType: System.Reflection.MemberTypes;
		ReturnType: System.Type;
		ReturnParameter: System.Reflection.ParameterInfo;
		ReturnTypeCustomAttributes: System.Reflection.ICustomAttributeProvider;
	}
	interface ParameterInfo {
		ParameterType: System.Type;
		Name: string;
		HasDefaultValue: boolean;
		DefaultValue: any;
		RawDefaultValue: any;
		Position: number;
		Attributes: System.Reflection.ParameterAttributes;
		Member: System.Reflection.MemberInfo;
		IsIn: boolean;
		IsOut: boolean;
		IsLcid: boolean;
		IsRetval: boolean;
		IsOptional: boolean;
		MetadataToken: number;
		CustomAttributes: System.Reflection.CustomAttributeData[];
	}
	interface ICustomAttributeProvider {
	}
	interface TypeInfo extends System.Type {
		GenericTypeParameters: System.Type[];
		DeclaredConstructors: System.Reflection.ConstructorInfo[];
		DeclaredEvents: System.Reflection.EventInfo[];
		DeclaredFields: System.Reflection.FieldInfo[];
		DeclaredMembers: System.Reflection.MemberInfo[];
		DeclaredMethods: System.Reflection.MethodInfo[];
		DeclaredNestedTypes: System.Reflection.TypeInfo[];
		DeclaredProperties: System.Reflection.PropertyInfo[];
		ImplementedInterfaces: System.Type[];
	}
	interface EventInfo extends System.Reflection.MemberInfo {
		MemberType: System.Reflection.MemberTypes;
		Attributes: System.Reflection.EventAttributes;
		AddMethod: System.Reflection.MethodInfo;
		RemoveMethod: System.Reflection.MethodInfo;
		RaiseMethod: System.Reflection.MethodInfo;
		EventHandlerType: System.Type;
		IsSpecialName: boolean;
		IsMulticast: boolean;
	}
	interface FieldInfo extends System.Reflection.MemberInfo {
		MemberType: System.Reflection.MemberTypes;
		FieldHandle: System.RuntimeFieldHandle;
		FieldType: System.Type;
		Attributes: System.Reflection.FieldAttributes;
		IsPublic: boolean;
		IsPrivate: boolean;
		IsFamily: boolean;
		IsAssembly: boolean;
		IsFamilyAndAssembly: boolean;
		IsFamilyOrAssembly: boolean;
		IsStatic: boolean;
		IsInitOnly: boolean;
		IsLiteral: boolean;
		IsNotSerialized: boolean;
		IsSpecialName: boolean;
		IsPinvokeImpl: boolean;
		IsSecurityCritical: boolean;
		IsSecuritySafeCritical: boolean;
		IsSecurityTransparent: boolean;
	}
	interface PropertyInfo extends System.Reflection.MemberInfo {
		MemberType: System.Reflection.MemberTypes;
		PropertyType: System.Type;
		Attributes: System.Reflection.PropertyAttributes;
		CanRead: boolean;
		CanWrite: boolean;
		GetMethod: System.Reflection.MethodInfo;
		SetMethod: System.Reflection.MethodInfo;
		IsSpecialName: boolean;
	}
	interface Binder {
	}
}
declare module System.Runtime.InteropServices {
	interface StructLayoutAttribute extends System.Attribute {
		Value: System.Runtime.InteropServices.LayoutKind;
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
	interface FileDetail {
		FilePath: string;
		Data: number[];
	}
}
declare module sharpbox.Membership.Model {
	interface UserRole extends sharpbox.Common.Type.EnumPattern {
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
	interface BackLogItem {
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
declare module sharpbox.WebLibrary.Core {
	interface WebRequest<T> {
		UiAction: sharpbox.WebLibrary.Helpers.UiAction;
		CommandName: sharpbox.Common.Dispatch.Model.CommandName;
		Instance: T;
	}
	interface WebResponse<T> {
		Instance: T;
		ModelErrors: System.Collections.Generic.KeyValuePair<string, System.Web.Mvc.ModelError[]>[];
		ResponseType: string;
		Message: string;
	}
}
declare module sharpbox.WebLibrary.Helpers {
	interface UiAction extends sharpbox.Common.Type.EnumPattern {
		Name: string;
	}
}
declare module System.Collections.Generic {
	interface KeyValuePair<TKey, TValue> {
		Key: TKey;
		Value: TValue;
	}
}
declare module System.Web.Mvc {
	interface ModelError {
		Exception: System.Exception;
		ErrorMessage: string;
	}
}


module sharpbox.Dispatch.Model {
	export const enum ResponseTypes {
		Info = 0,
		Warning = 1,
		Success = 2,
		Error = 3
	}
}
module System.Reflection {
	export const enum MethodImplAttributes {
		CodeTypeMask = 3,
		IL = 0,
		Native = 1,
		OPTIL = 2,
		Runtime = 3,
		ManagedMask = 4,
		Unmanaged = 4,
		Managed = 0,
		ForwardRef = 16,
		PreserveSig = 128,
		InternalCall = 4096,
		Synchronized = 32,
		NoInlining = 8,
		AggressiveInlining = 256,
		NoOptimization = 64,
		MaxMethodImplVal = 65535
	}
	export const enum MethodAttributes {
		MemberAccessMask = 7,
		PrivateScope = 0,
		Private = 1,
		FamANDAssem = 2,
		Assembly = 3,
		Family = 4,
		FamORAssem = 5,
		Public = 6,
		Static = 16,
		Final = 32,
		Virtual = 64,
		HideBySig = 128,
		CheckAccessOnOverride = 512,
		VtableLayoutMask = 256,
		ReuseSlot = 0,
		NewSlot = 256,
		Abstract = 1024,
		SpecialName = 2048,
		PinvokeImpl = 8192,
		UnmanagedExport = 8,
		RTSpecialName = 4096,
		ReservedMask = 53248,
		HasSecurity = 16384,
		RequireSecObject = 32768
	}
	export const enum CallingConventions {
		Standard = 1,
		VarArgs = 2,
		Any = 3,
		HasThis = 32,
		ExplicitThis = 64
	}
	export const enum MemberTypes {
		Constructor = 1,
		Event = 2,
		Field = 4,
		Method = 8,
		Property = 16,
		TypeInfo = 32,
		Custom = 64,
		NestedType = 128,
		All = 191
	}
	export const enum ParameterAttributes {
		None = 0,
		In = 1,
		Out = 2,
		Lcid = 4,
		Retval = 8,
		Optional = 16,
		ReservedMask = 61440,
		HasDefault = 4096,
		HasFieldMarshal = 8192,
		Reserved3 = 16384,
		Reserved4 = 32768
	}
	export const enum EventAttributes {
		None = 0,
		SpecialName = 512,
		ReservedMask = 1024,
		RTSpecialName = 1024
	}
	export const enum FieldAttributes {
		FieldAccessMask = 7,
		PrivateScope = 0,
		Private = 1,
		FamANDAssem = 2,
		Assembly = 3,
		Family = 4,
		FamORAssem = 5,
		Public = 6,
		Static = 16,
		InitOnly = 32,
		Literal = 64,
		NotSerialized = 128,
		SpecialName = 512,
		PinvokeImpl = 8192,
		ReservedMask = 38144,
		RTSpecialName = 1024,
		HasFieldMarshal = 4096,
		HasDefault = 32768,
		HasFieldRVA = 256
	}
	export const enum PropertyAttributes {
		None = 0,
		SpecialName = 512,
		ReservedMask = 62464,
		RTSpecialName = 1024,
		HasDefault = 4096,
		Reserved2 = 8192,
		Reserved3 = 16384,
		Reserved4 = 32768
	}
	export const enum TypeAttributes {
		VisibilityMask = 7,
		NotPublic = 0,
		Public = 1,
		NestedPublic = 2,
		NestedPrivate = 3,
		NestedFamily = 4,
		NestedAssembly = 5,
		NestedFamANDAssem = 6,
		NestedFamORAssem = 7,
		LayoutMask = 24,
		AutoLayout = 0,
		SequentialLayout = 8,
		ExplicitLayout = 16,
		ClassSemanticsMask = 32,
		Class = 0,
		Interface = 32,
		Abstract = 128,
		Sealed = 256,
		SpecialName = 1024,
		Import = 4096,
		Serializable = 8192,
		WindowsRuntime = 16384,
		StringFormatMask = 196608,
		AnsiClass = 0,
		UnicodeClass = 65536,
		AutoClass = 131072,
		CustomFormatClass = 196608,
		CustomFormatMask = 12582912,
		BeforeFieldInit = 1048576,
		ReservedMask = 264192,
		RTSpecialName = 2048,
		HasSecurity = 262144
	}
	export const enum GenericParameterAttributes {
		None = 0,
		VarianceMask = 3,
		Covariant = 1,
		Contravariant = 2,
		SpecialConstraintMask = 28,
		ReferenceTypeConstraint = 4,
		NotNullableValueTypeConstraint = 8,
		DefaultConstructorConstraint = 16
	}
}
module System.Runtime.InteropServices {
	export const enum LayoutKind {
		Sequential = 0,
		Explicit = 2,
		Auto = 3
	}
}
module System.Security {
	export const enum SecurityRuleSet {
		None = 0,
		Level1 = 1,
		Level2 = 2
	}
}

