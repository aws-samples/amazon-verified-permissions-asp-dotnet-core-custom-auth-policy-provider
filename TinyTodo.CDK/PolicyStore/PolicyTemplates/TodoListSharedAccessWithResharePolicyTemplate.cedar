permit (
    principal in ?principal,
    action in
        [TinyTodoList::Action::"AddTodoItem",
         TinyTodoList::Action::"ShareTodoList"],
    resource in ?resource
);