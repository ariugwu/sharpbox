# sharpbox
A group of common intranet application components (modules) that are encapsulated within a (app) context and communicate via a dispatcher.

## Modules 
All modules except for Dispatch utilize the strategy pattern. Dispatch uses a pub/sub pattern to centralize communication.

### Audit
Can be configured via Audit Levels. For example the 'All' level will register a callback for all available publications. When in this state all actions from the other modules will be logged. (The actual audit action will be ignored to prevent looping)

### Email

### Log

### Data
Consists of a generic repository pattern with to build in strategies: XML and EF

### Membership

### Io

### Dispatch
Inspired by the Facebook Flux architecture this module is common to all enteral modules and can the list of available publishers can be extended by the calling layer.

### Data
Generic repository with two baked in strategies: XML and EF
Currently XML is the default storage for the Audit module.
