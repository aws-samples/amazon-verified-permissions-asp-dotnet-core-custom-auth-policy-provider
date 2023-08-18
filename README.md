## Implement custom authorization policy provider for ASP.NET Core apps using Amazon Verified Permissions

This is a sample asp.net core application with a custom authorization policy provider which makes use of Amazon Verified Permissions API to evaluate authorization requirements and obtain authorization result. 

Blog reference: [Implement custom authorization policy provider for ASP.NET Core apps using Amazon Verified Permissions](https://aws.amazon.com/blogs/dotnet/implement-a-custom-authorization-policy-provider-for-asp-net-core-apps-using-amazon-verified-permissions)

### The Architecture

- The sample asp.net core application uses a SQLite database to store entities and uses Amazon Verified Permissions as the centralized authorization service and authorization policy repository.

- When a user shares a to-do list with another user, the application creates a template-linked policy based on the shared access policy templates ([TodoListSharedAccessPolicyTemplate](/TinyTodo.CDK/PolicyStore/PolicyTemplates/TodoListSharedAccessPolicyTemplate.cs) / [TodoListSharedAccessWithResharePolicyTemplate](/TinyTodo.CDK/PolicyStore/PolicyTemplates/TodoListSharedAccessWithResharePolicyTemplate.cs))

![Architecture](/Media/Architecture.JPG)


## Prerequisites

To test the sample application, you need:
- An [AWS](https://console.aws.amazon.com/) account
- Access to the following AWS services: Amazon Verified Permissions.
- Policy Store created on Amazon Verified Permissions
- [Node.js](https://nodejs.org/en/download/) installed
- [AWS CDK](https://docs.aws.amazon.com/cdk/v2/guide/getting_started.html#getting_started_install) installed
- .NET 6.0 SDK installed
- JetBrains Rider or Microsoft Visual Studio 2017 or later (or Visual Studio Code)

## Deployment

To deploy the policy store on AWS:

- Build [TinyTodo.CDK](/TinyTodo.CDK/) project 
```cmd
C:\Dev\customauthpolicyproviderdemo\TinyTodo.CDK>dotnet build
```
- Deploy the CDK project
```cmd 
C:\Dev\customauthpolicyproviderdemo\TinyTodo.CDK>cdk deploy
```

- Copy the policy store and policy template ids from the CDK output

![CDK-Output](/Media/CDK-Output.png)

- Update the [appsettings.json](/TinyTodo.Web/appsettings.json) file in [TinyTodo.Web](/TinyTodo.Web/) project

![AppSettings](/Media/AppSettings.PNG)

## Testing


Run [TinyTodo.Web](/TinyTodo.Web/) project


### Resource based authorization

#### Create and share a sample todo list
* Login as user1@example.com (with any password), go to 'My Todo Lists' page.
* Create a to-do list
* Share it with user2@example.com (Leave 'Allow Reshare' option unchecked)
* Log out

#### Check the template-linked policy created
* Login to Amazon Verified Permissions Console
* Select the application's policy store
* You can see that a new template-linked policy is created (linked to the policy template [TodoListSharedAccessPolicyTemplate](/TinyTodo.CDK/PolicyStore/PolicyTemplates/TodoListSharedAccessPolicyTemplate.cs))

#### Verify the user permissions on the shared to-do list
* Login as user2@example.com (with any password), go to 'My Todo Lists' page.
* You can see the to-do list shared by user1@example.com
* Try resharing the to-do list with another user (user3@example.com), you should see an error message as the above mentioned template-linked policy grants permissions only to add items to the todo list.

### Attribute-based authorization

* Login as user2@example.com (with any password), go to 'My Todo Lists' page.
* You can see the to-do list shared by user1@example.com
* Try deleting the to-do list or sharing with anothe user (user3@example.com), you should see an error message (as limited by the policy [AllowActionsOnUserTodoListsPolicy](/TinyTodo.CDK/PolicyStore/StaticPolicies/AllowActionsOnUserTodoListsPolicy.cs))

### Role-based authorization

* Login as user1@example.com (with any password), try visiting 'Admin' page.
* You should see an error page (as limited by the policy [AdminModuleAccessPolicy](/TinyTodo.CDK/PolicyStore/StaticPolicies/AdminModuleAccessPolicy.cs))
* Logout
* Login as admin@example.com (with any password), try visiting 'Admin' page.
* You should see the admin page without any errors

## Reset the sample application

* To reset the application, just rename/delete the TinyTodoDatabase.db file in [TinyTodo.Web](/TinyTodo.Web/) project 

## Resources

* [Cedar policy language](https://docs.cedarpolicy.com/)
* [Amazon.CDK.AwsVerifiedpermissions](https://docs.aws.amazon.com/cdk/api/v2/dotnet/api/Amazon.CDK.AwsVerifiedpermissions.html)

### References

* [Using Open Source Cedar to Write and Enforce Custom Authorization Policies](https://aws.amazon.com/blogs/opensource/using-open-source-cedar-to-write-and-enforce-custom-authorization-policies/) blog post
* [Policy-based access control in application development with Amazon Verified Permissions](https://aws.amazon.com/blogs/devops/policy-based-access-control-in-application-development-with-amazon-verified-permissions/) blog post
* [Custom Authorization Policy Providers using IAuthorizationPolicyProvider in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/iauthorizationpolicyprovider?view=aspnetcore-6.0)

## Security

See [CONTRIBUTING](CONTRIBUTING.md#security-issue-notifications) for more information.

## License

This library is licensed under the MIT-0 License. See the LICENSE file.