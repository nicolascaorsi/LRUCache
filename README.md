**SimpleLRUCache** is a simple implementation of [LRU algorithm](https://en.wikipedia.org/wiki/Cache_algorithms#LRU) for .NET.

[![NuGet](https://img.shields.io/nuget/dt/Microsoft.AspNet.Mvc.svg)](https://www.nuget.org/packages/SimpleLRUCache/)
[![Build status](https://ci.appveyor.com/api/projects/status/cr6hwdgf3xwqffcc/branch/master?svg=true)](https://ci.appveyor.com/project/nicolascaorsi/lrucache/branch/master)
## Content

* [Getting started](#getting-started)
* [How to use?](#how-to-use)
* [Authors](#authors)

## Getting started

Install SimpleLRUCache via the NuGet package: [SimpleLRUCache](https://www.nuget.org/packages/SimpleLRUCache/)

```
PM> Install-Package SimpleLRUCache
```

## How to use
There are two types of LRU cache, LRUCache and LRUCacheDisposable, which is a version of LRUCache who calls dispose, when a item is removed from cache.

Suppose you are working with a list of users, and wants to cache the last 100 recent accessed objects.
```cs
public class UsersService
{
    private readonly LRUCache<string, User> usersLRU;
    
    public UsersService()
    {
      // Creates a LRUCache from users object, with a limit of 100 items.
      this.usersLRU = new LRUCache<string,User>(100);
    }
    
    public User GetUser(int id)
    {
      var foundUser = usersLRU.Get(id);
      // if user isn't in cache, then we retrieve from persistence
      if(foundUser == null)
      {
        foundUser = usersPersistence.Get(id);
        // if user exists we add him to the list
        if(foundUser != null)
        {
          // key and the object to store
          usersLRU.Add(id, foundUser);
        }
      }
      return foundUser;
    }
}
```

If the class User, form above example, implements a Disposable interface, then you should use the LRUCacheDisposable. So when the list reaches the established limit, the Dispose method is called, before item removal.
Exemple from above using LRUCacheDisposable:
```cs
public class UsersService
{
    private readonly LRUCacheDisposable<string, User> usersLRU;
    
    public UsersService()
    {
      // Creates a LRUCache from users object, with a limit of 100 items.
      this.usersLRU = new LRUCacheDisposable<string,User>(100);
    }

    …
}
```

## Authors

Nicolás Caorsi (2015)
