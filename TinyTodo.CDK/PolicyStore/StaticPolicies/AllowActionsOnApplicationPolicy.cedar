permit (
    principal,
    action in
        [TinyTodoList::Action::"GetTodoLists",
         TinyTodoList::Action::"CreateTodoList"],
    resource == TinyTodoList::Application::"TinyTodoListApp"
);