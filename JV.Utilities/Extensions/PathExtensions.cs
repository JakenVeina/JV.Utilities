using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JV.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Path"/>.
    /// </summary>
    public static class PathExtensions
    {
        /// <summary>
        /// <para>
        /// The inverse operation of <see cref="Path.Combine(string[])"/>, in the same fashion as <see cref="String.Split(string[], StringSplitOptions)"/>.
        /// </para>
        /// <para>
        /// Splits the given path into an array of individual file-system elements, which, when passed to <see cref="Path.Combine(string[])"/>
        /// will produce a string semantically identical to the original given path. Note that this method is NOT equivalent to
        /// <see cref="String.Split(string[], StringSplitOptions)"/> with <see cref="Path.DirectorySeparatorChar"/> as the delimiter, because
        /// <see cref="Path.Combine(string[])"/> handles root file-system elements differently than normal elements.
        /// </para>
        /// <para>
        /// Additionally, this method does not validate the given path, except to ensure that it contains no invalid characters.
        /// As such, its behavior is undefined if it is given a non-well-formed path.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException"> for path.</exception>
        /// <exception cref="ArgumentException">
        /// Throws if path contains any invalid characters, defined in <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <param name="path">The path to be split.</param>
        /// <returns>Array containing each separate file-system element from the given path.</returns>
        public static string[] Split(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var separators = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

            // If there's no root, just split the path by directory separators
            // This also validates that path does not contain any invalid characters.
            try
            {
                if (!Path.IsPathRooted(path))
                    return path.Split(separators);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message, nameof(path), e);
            }

            // If there is a root, remove it, split the rest of the path normally, then insert the root onto the front of the result.
            var root = Path.GetPathRoot(path);
            var splitItems = path.Remove(0, root.Length).Split(separators);
            var result = new string[splitItems.Length + 1];
            result[0] = root;
            splitItems.CopyTo(result, 1);
            return result;
        }
        
        /// <summary>
        /// Overload for <see cref="GetCommonPath(IEnumerable{string})"/> that accepts a params array of strings.
        /// </summary>
        public static string GetCommonPath(params string[] paths) => GetCommonPath((IEnumerable<string>)paths);

        /// <summary>
        /// Returns the path to the lowest-level element that is included in all of the given paths.
        /// This includes the documentName portion of the path, if all given paths are the same, or if there is only one.
        /// Empty and null paths are ignored.
        /// 
        /// I.E. "C:\Directory1\Directory2\file1"
        ///      "C:\Directory1\Directory2\file2" => "C:\Directory1\Directory2"
        ///      
        ///      "C:\Directory1\Directory2\file"
        ///      "C:\Directory1\Directory2"
        ///      "C:\Directory1"                  => "C:\Directory1"
        ///      
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for paths.</exception>
        /// <param name="paths">A list of strings for which the common path is to be determined.</param>
        /// <returns>The portion of each given path that is shared by all the given paths.</returns>
        public static string GetCommonPath(IEnumerable<string> paths)
        {
            if (paths == null)
                throw new ArgumentNullException(nameof(paths));

            // Enumerators for walking each path in parallel.
            // Each path is split into an array of directories, and an enumerator is built to walk through them, starting at the root.
            IEnumerable<IEnumerator<string>> enumerators;
            try
            {
                enumerators = paths.Select(x => ((IEnumerable<string>)Split(x)).GetEnumerator()).ToList();
            }
            catch(ArgumentNullException ex)
            {
                throw new ArgumentException("Cannot contain a null path", nameof(paths), ex);
            }

            // There's no exposed property for the CLR's maximum path length, but it should be 260 on windows systems,
            // so we should never see a folder depth greater than 130. Worst-case-scenario, if this assumption fails,
            // the list will just re-allocate itself as needed.
            var commonElements = new List<string>(130);

            // Iterate through each element in all the paths, until we reach the end of one of the paths,
            // or we find an element that isn't the same across all of them.
            while(enumerators.All(x => x.MoveNext()))
            {
                // Select the folder name from each path, at the current level, and filter out duplicates.
                var distinctElements = enumerators.Select(x => x.Current).Distinct();
                
                // If there's only one unique folder name, add it to the result and keep going.
                if (distinctElements.Any() && !distinctElements.Skip(1).Any())
                    commonElements.Add(distinctElements.First());
                
                // Otherwise, we're done.
                else
                    break;
            }

            // Each folder has already been validated by Split(), so we can optimize by just joining them all.
            return Path.Combine(commonElements.ToArray());
        }
    }
}
