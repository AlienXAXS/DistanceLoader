using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DistanceLoader.Util
{
    public class ObjectDumper
    {
        private int _level;
        private readonly int _indentSize;
        private readonly StringBuilder _stringBuilder;
        private readonly List<int> _hashListOfFoundElements;
        private readonly bool _dumpParentObject;


        public static void DumpGameObjects(GameObject[] objects, int depth = -1)
        {
            foreach (var child in objects)
                DumpGameObjects(child, false, depth:depth);
        }

        public static void DumpGameObjects(GameObject root, bool dumpParent = true, string path = "", int depth = -1)
        {
            int currentDepth = 0;
            int currentDepthInner = 0;

            // Log out the root components first, we need those :)
            var rootComponents = new List<UnityEngine.Component>();
            foreach (var component in root.GetComponents<UnityEngine.Component>())
            {
                Util.Logger.Instance.Log($"[DumpGameObjects]\r\n=========== {path} (Comp) ===========\r\n{Util.ObjectDumper.Dump(component, 5, dumpParent)}");

                currentDepth++;
                if (depth != -1 && currentDepth > depth)
                    break;
            }

            currentDepth = 0;
            foreach (var child in root.GetChildren())
            {
                path = $"{path} -> {child.name}";
                Util.Logger.Instance.Log($"[DumpGameObjects]\r\n=========== {path} ===========\r\n{Util.ObjectDumper.Dump(child, 5, dumpParent)}");

                var components = new List<UnityEngine.Component>();
                foreach (var component in child.GetComponents<UnityEngine.Component>())
                {
                    Util.Logger.Instance.Log($"[DumpGameObjects]\r\n=========== {path} (Comp) ===========\r\n{Util.ObjectDumper.Dump(component, 5, dumpParent)}");

                    currentDepthInner++;
                    if (depth != -1 && currentDepthInner > depth)
                        break;
                }

                DumpGameObjects(child, dumpParent, path, depth);

                currentDepth++;
                if (depth != -1 && currentDepth > depth)
                    break;

                currentDepthInner = 0;
            }
        }

        private ObjectDumper(int indentSize, bool _dumpParent)
        {
            _indentSize = indentSize;
            _stringBuilder = new StringBuilder();
            _hashListOfFoundElements = new List<int>();
            _dumpParentObject = _dumpParent;
        }

        public static string Dump(object element, bool dumpParent = true)
        {
            return Dump(element, 2, dumpParent);
        }

        public static string Dump(object element, int indentSize, bool dumpParent)
        {
            var instance = new ObjectDumper(indentSize, dumpParent);
            return instance.DumpElement(element);
        }

        private string DumpElement(object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                Write(FormatValue(element));
            }
            else
            {
                var objectType = element.GetType();
                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    Write("{{{0}}}", objectType.FullName);
                    _hashListOfFoundElements.Add(element.GetHashCode());
                    _level++;
                }

                if (element is IEnumerable enumerableElement)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            _level++;
                            DumpElement(item);
                            _level--;
                        }
                        else
                        {
                            if (!AlreadyTouched(item))
                                DumpElement(item);
                            else
                                Write("{{{0}}} <-- bidirectional reference found", item.GetType().FullName);
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var memberInfo in members)
                    {
                        try
                        {
                            var fieldInfo = memberInfo as FieldInfo;
                            var propertyInfo = memberInfo as PropertyInfo;

                            if (fieldInfo == null && propertyInfo == null)
                                continue;

                            if (memberInfo.Name.Equals("parent", StringComparison.OrdinalIgnoreCase) && !_dumpParentObject) continue;

                            var type = fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;
                            object value = fieldInfo != null
                                ? fieldInfo.GetValue(element)
                                : propertyInfo.GetValue(element, null);

                            if (type.IsValueType || type == typeof(string))
                            {
                                Write("{0}: {1}", memberInfo.Name, FormatValue(value));
                            }
                            else
                            {
                                var isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);
                                Write("{0}: {1}", memberInfo.Name, isEnumerable ? "..." : "{ }");

                                var alreadyTouched = !isEnumerable && AlreadyTouched(value);
                                _level++;
                                if (!alreadyTouched)
                                    DumpElement(value);
                                else
                                    Write("{{{0}}} <-- bidirectional reference found", value.GetType().FullName);
                                _level--;
                            }
                        }
                        catch (Exception ex)
                        {
                            //Util.Logger.Instance.Log($"[DumpElement] Exception", ex);
                        }
                    }
                }

                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    _level--;
                }
            }

            return _stringBuilder.ToString();
        }

        private bool AlreadyTouched(object value)
        {
            if (value == null)
                return false;

            var hash = value.GetHashCode();
            for (var i = 0; i < _hashListOfFoundElements.Count; i++)
            {
                if (_hashListOfFoundElements[i] == hash)
                    return true;
            }
            return false;
        }

        private void Write(string value, params object[] args)
        {
            var space = new string(' ', _level * _indentSize);

            if (args != null)
                value = string.Format(value, args);

            _stringBuilder.AppendLine(space + value);
        }

        private string FormatValue(object o)
        {
            if (o == null)
                return ("null");

            if (o is DateTime)
                return (((DateTime)o).ToShortDateString());

            if (o is string)
                return string.Format("\"{0}\"", o);

            if (o is char && (char)o == '\0')
                return string.Empty;

            if (o is ValueType)
                return (o.ToString());

            if (o is IEnumerable)
                return ("...");

            return ("{ }");
        }
    }
}
