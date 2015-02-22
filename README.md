# sharpbox
A group of common intranet application components (modules) that are encapsulated within a (app) context and communicate via a dispatcher. The dispatcher and the concept of a unidirectional data flow come from the [Facebook Flux Architecture](http://facebook.github.io/flux/docs/overview.html).

Where the simplicity of that approach fails the concepts of [Event Sourcing] (https://msdn.microsoft.com/en-us/library/jj591559.aspx) will come into play.

## Example (@SEE CLI example project):

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
## The problem it solves:

#### Turn this
```c#
public static T SomeUnitOfWork(T SomeObject, DependencyA depA, DependencyB depB){
    // Do Stuff to Object
    someObject.DoStuffExtension();
    // Run a check
    if(someObject.ResultOfExtension > 5){
      // Do Other stuff.
    }
    
    // Call the first dependency to do something like send email.
    depA.UpdateWithStatusOfSomeObject(someObject);
    
    // Call the second dependency for whatever reason
    someObject = depB.PersistObject(someObject);
    
    return SomeObject;
}

static void Main(string[] args){
  var something = new SomeObject();
  var depA = new DependencyA();
  var debB = new DependencyB();
  something = SomeUnitOfWork(something, depA, depB);
}
```

#### Into this

```c#
public static T SomeUnitOfWork(Dispatcher dispatch, T someObject){
    // Do Stuff to Object
    someObject.DoStuffExtension();
    dispatch.BroadCast(DoStuffExtensionFired, someObject);
    return someObject;
}

static void Main(string[] args){
  var dispatch = new Dispatch();
  var depA = new DependencyA();
  var debB = new DependencyB();
  var someObject = new SomeObject();
  var container = EncapsulateComponents(dispatch, depA, debB, someObject);
  
  // Tell the dispatch that whenever a request to fire is sent for 'FireSomeUnitOfWork' to pass the request to the 'SomeUnitOfWork' callback.
  dispatch.Register(FireSomeUnitOfWork, SomeUnitOfWork);
  
  // Once the SomeUnitOfWork fires it will broadcast to anyone listening. Below we'll register some listeners.
  dispatch.Listen(DoStuffExtensionFired, container.UpdateSomethingObject);
  dispatch.Listen(DoStuffExtensionFired, depA.UpdateUsersWithStatusOfSomeObject);
  dispatch.Listen(DoStuffExtensionFired, depB.PersistObject);
  
  // Send a request to process the object in our container.
  dispatch.Process(FireSomeUnitOfWork, container.Something);
  
  // It's our containers job to pass the new values of someObject to any components that might need it.
  Debug.WriteLine(container.Something.MutableProperty);
  
}
```

The benefit (hopefully) is that we can maintain single responsbility throughout the system. At the same time we can have clean central auditing and extend the system from the calling layer instead of nesting functionality in business logic. 

Keep in the mind that the first example gets *more* complicated in a real example as you add error handling, roll backs, and such. The second example gets *less* complicated as each channel is self contained.
