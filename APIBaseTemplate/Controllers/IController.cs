using APIBaseTemplate.Common;
using Microsoft.AspNetCore.Mvc;

namespace APIBaseTemplate.Controllers
{
    public interface IController<T>
        where T : new()
    {
        /// <summary>
        /// Return specific <see cref="T"/> by id
        /// </summary>
        /// <param name="request">EntityId</param>
        /// <returns>Entity with details</returns>
        ResponseOf<T> GetById([FromBody] RequestOf<EntityNumberIdParam> request);

        /// <summary>
        /// Returns available sorting parameters
        /// </summary>
        /// <returns></returns>
        ResponseOf<List<string>> GetSortingParameters();

        /// <summary>
        /// Save a <see cref="T"/>
        /// </summary>
        /// <param name="request">item to update or insert</param>
        /// <remarks>
        /// If entity's id has a value an update will be performed.
        /// If entity's id doesn't have value an insert will be performed.
        /// </remarks>
        /// <returns>The updated/inserted <see cref="T"/> </returns>
        ResponseOf<T> Save([FromBody] RequestOf<T> request);

        /// <summary>
        /// Delete an <see cref="T"/>
        /// </summary>
        /// <param name="req">The entity that must be deleted</param>
        /// <returns>Empty response if ok</returns>
        public Response Delete([FromBody] RequestOf<EntityNumberIdParam> req);
    }
}
