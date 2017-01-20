﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using WireMock.Validation;

namespace WireMock.Matchers.Request
{
    /// <summary>
    /// The request header matcher.
    /// </summary>
    public class RequestMessageHeaderMatcher : IRequestMatcher
    {
        /// <summary>
        /// The name.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// The patternRegex.
        /// </summary>
        private readonly Regex _patternRegex;

        /// <summary>
        /// The header function
        /// </summary>
        private readonly Func<IDictionary<string, string>, bool> _headerFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessageHeaderMatcher"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="ignoreCase">The ignoreCase.</param>
        public RequestMessageHeaderMatcher([NotNull] string name, [NotNull, RegexPattern] string pattern, bool ignoreCase = true)
        {
            _name = name;
            _patternRegex = ignoreCase ? new Regex(pattern, RegexOptions.IgnoreCase) : new Regex(pattern);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessageHeaderMatcher"/> class.
        /// </summary>
        /// <param name="func">
        /// The func.
        /// </param>
        public RequestMessageHeaderMatcher([NotNull] Func<IDictionary<string, string>, bool> func)
        {
            Check.NotNull(func, nameof(func));
            _headerFunc = func;
        }

        /// <summary>
        /// Determines whether the specified RequestMessage is match.
        /// </summary>
        /// <param name="requestMessage">The RequestMessage.</param>
        /// <returns>
        ///   <c>true</c> if the specified RequestMessage is match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(RequestMessage requestMessage)
        {
            if (_patternRegex == null)
                return _headerFunc(requestMessage.Headers);

            string headerValue = requestMessage.Headers[_name];
            return _patternRegex.IsMatch(headerValue);
        }
    }
}