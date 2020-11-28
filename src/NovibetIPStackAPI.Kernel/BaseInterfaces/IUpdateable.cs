using System;

namespace NovibetIPStackAPI.Kernel.Interfaces
{
    /// <summary>
    /// An interface signaling that the entity that implements it is updateable. It contains datetime information on when it was created and last modified and a unique identifier stored as long.
    /// </summary>
    public interface IUpdateable
    {
        long Id { get; set; }

        DateTime DateCreated { get; set; }

        DateTime DateLastModified { get; set; }

    }
}
