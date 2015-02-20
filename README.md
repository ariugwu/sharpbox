# sharpbox
A group of common intranet application components (modules) that are encapsulated within a (app) context and communicate via a dispatcher. The dispatcher and the concept of a unidirectional data flow come from the [Facebook Flux Architecture](http://facebook.github.io/flux/docs/overview.html).

The basic approach looks like this:

```c#
var smtpClient = new SmtpClient("smtp.google.com", 587);
var app = new ConsoleContext("ugwua", PublicationNamesExtension.ExtendedPubList, ActionNames.DefaultActionList(), smtpClient);

// Register a callback for the 'SetFeedback' action
app.Dispatch.Register(ActionNames.SetFeedback, app.ExampleProcessFeedback);

// Create a new feedback message and ask the dispatcher to process it.
var feedback = new Feedback{ ActionName = ActionNames.ChangeUser, Message = "Meaningless message", Successful = true};

// The goal is to only have one primary action do the processing. Other system monitoring events may also be called (auditing for example).
app.Dispatch.Process(new Request{ ActionName = ActionNames.SetFeedback, Message = "A test to set the feedback", Entity = feedback, RequestId = 0, Type = typeof(Feedback), UserId = app.Dispatch.CurrentUserId});

// In this example the app (application context) is the store (Flux term) and container for it's own components. So it's // responsible for processing actions and passing those updates to its components.
```
