permit (
    principal,
    action in
        [TinyTodoList::Action::"AddTodoItem",
         TinyTodoList::Action::"ShareTodoList",
         TinyTodoList::Action::"DeleteTodoList",
         TinyTodoList::Action::"MakeTodoListPrivate"],
    resource
)
when { resource.Owner == principal.Email };