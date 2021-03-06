//  Copyright (c) 2012 Calvin Rien
//        http://the.darktable.com
//
// This software is provided 'as-is', without any express or implied warranty. In
// no event will the authors be held liable for any damages arising from the use
// of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it freely,
// subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim
// that you wrote the original software. If you use this software in a product,
// an acknowledgment in the product documentation would be appreciated but is not
// required.
//
// 2. Altered source versions must be plainly marked as such, and must not be
// misrepresented as being the original software.
//
// 3. This notice may not be removed or altered from any source distribution.

using UnityEngine;
using System.Collections.Generic;

namespace Lerp2API.Utility
{
    /// <summary>
    /// Class ObjectKvp. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Lerp2API.Utility.UnityNameValuePair{UnityEngine.Object}" />
    [System.Serializable]
    public sealed class ObjectKvp : UnityNameValuePair<Object>
    {
        /// <summary>
        /// The value
        /// </summary>
        public Object value = null;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        override public Object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectKvp"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public ObjectKvp(string key, Object value) : base(key, value)
        {
        }
    }

    /// <summary>
    /// Class ObjectDictionary.
    /// </summary>
    /// <seealso cref="Lerp2API.Utility.UnityDictionary{UnityEngine.Object}" />
    [System.Serializable]
    public class ObjectDictionary : UnityDictionary<Object>
    {
        /// <summary>
        /// The values
        /// </summary>
        public List<ObjectKvp> values;

        /// <summary>
        /// Gets or sets the key value pairs.
        /// </summary>
        /// <value>The key value pairs.</value>
        override protected List<UnityKeyValuePair<string, Object>> KeyValuePairs
        {
            get
            {
                return values.ConvertAll<UnityKeyValuePair<string, Object>>(new System.Converter<ObjectKvp, UnityKeyValuePair<string, Object>>(
                x =>
                {
                    return x as UnityKeyValuePair<string, Object>;
                }));
            }
            set
            {
                if (value == null)
                {
                    values = new List<ObjectKvp>();
                    return;
                }

                values = value.ConvertAll<ObjectKvp>(new System.Converter<UnityKeyValuePair<string, Object>, ObjectKvp>(
                x =>
                {
                    return new ObjectKvp(x.Key, x.Value as Object);
                }));
            }
        }

        /// <summary>
        /// Sets the key value pair.
        /// </summary>
        /// <param name="k">The k.</param>
        /// <param name="v">The v.</param>
        override protected void SetKeyValuePair(string k, Object v)
        {
            var index = values.FindIndex(x =>
            {
                return x.Key == k;
            });

            if (index != -1)
            {
                if (v == null)
                {
                    values.RemoveAt(index);
                    return;
                }

                values[index] = new ObjectKvp(k, v);
                return;
            }

            values.Add(new ObjectKvp(k, v));
        }
    }
}