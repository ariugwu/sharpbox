# sharpbox
A group of common intranet application components (modules) that are encapsulated within a (app) context and communicate via a dispatcher. The dispatcher and the concept of a unidirectional data flow come from the [Facebook Flux Architecture](http://facebook.github.io/flux/docs/overview.html).

## Modules 
Most projects are simply thin wrappers for injecting the dispatcher dependency. The strategy pattern is also used heavily to allow for a good deal of implementation flexibility without compromising the integrity of the framework.

### Audit
Can be configured via Audit Levels. For example the 'All' level will register a callback for all available publications. When in this state all actions from the other modules will be logged. (The actual audit action will be ignored to prevent looping)

### Email
A thin wrapper for the System.Net.Mail namespace. Functionality may expand in the future.

### Log
Logging is very simple and differs from Auditing only in that it utilizies System.Runtime.CompilerServices to assist in the log entry formatting.

### Notification
Much like auditing this will register itself with all known publishers. It will keep a queue of all actions which have occured during a session. It will persist a backlog of messages to users who subcribed to various events.

### Io
I thin wrapper for the File namespace.

### Dispatch
Inspired by the Facebook Flux architecture this module is common to all enteral modules and can the list of available publishers can be extended by the calling layer.

### Data
Generic repository with two baked in strategies: XML and EF
Currently XML is the default storage for the Audit module.
