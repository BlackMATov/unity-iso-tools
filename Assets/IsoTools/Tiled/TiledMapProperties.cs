﻿using UnityEngine;
using System;
using System.Globalization;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IsoTools.Tiled {
	public class TiledMapProperties {
		List<string> _properties = null;

		// -----------------------------
		// Functions
		// -----------------------------

		public TiledMapProperties(List<string> properties) {
			_properties = properties;
		}

		public int Count {
			get {
				return _properties != null
					? _properties.Count / 2
					: 0;
			}
		}

		public string GetKeyByIndex(int index) {
			if ( (uint)index >= (uint)Count ) {
				throw new IndexOutOfRangeException();
			}
			return _properties[index * 2];
		}

		public string GetValueByIndex(int index) {
			if ( (uint)index >= (uint)Count ) {
				throw new IndexOutOfRangeException();
			}
			return _properties[index * 2 + 1];
		}

		public bool Has(string property_name) {
			if ( _properties != null ) {
				for ( int i = 0, e = _properties.Count / 2; i < e; ++i ) {
					if ( _properties[i * 2] == property_name ) {
						return true;
					}
				}
			}
			return false;
		}

		// -----------------------------
		// GetAsX
		// -----------------------------

		public bool GetAsBool(string property_name) {
			bool value;
			if ( TryGetAsBool(property_name, out value) ) {
				return value;
			}
			throw new UnityException("find or parse parameter error");
		}

		public short GetAsShort(string property_name) {
			short value;
			if ( TryGetAsShort(property_name, out value) ) {
				return value;
			}
			throw new UnityException("find or parse parameter error");
		}

		public int GetAsInt(string property_name) {
			int value;
			if ( TryGetAsInt(property_name, out value) ) {
				return value;
			}
			throw new UnityException("find or parse parameter error");
		}

		public long GetAsLong(string property_name) {
			long value;
			if ( TryGetAsLong(property_name, out value) ) {
				return value;
			}
			throw new UnityException("find or parse parameter error");
		}

		public float GetAsFloat(string property_name) {
			float value;
			if ( TryGetAsFloat(property_name, out value) ) {
				return value;
			}
			throw new UnityException("find or parse parameter error");
		}

		public double GetAsDouble(string property_name) {
			double value;
			if ( TryGetAsDouble(property_name, out value) ) {
				return value;
			}
			throw new UnityException("find or parse parameter error");
		}

		public string GetAsString(string property_name) {
			string value;
			if ( TryGetAsString(property_name, out value) ) {
				return value;
			}
			throw new UnityException("find or parse parameter error");
		}

		// -----------------------------
		// TryGetAsX
		// -----------------------------

		public bool TryGetAsBool(string property_name, out bool value) {
			string property_value;
			if ( TryGetAsString(property_name, out property_value) ) {
				if ( bool.TryParse(property_value, out value) ) {
					return true;
				}
			}
			value = false;
			return false;
		}

		public bool TryGetAsShort(string property_name, out short value) {
			string property_value;
			if ( TryGetAsString(property_name, out property_value) ) {
				if ( short.TryParse(property_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ) {
					return true;
				}
			}
			value = 0;
			return false;
		}

		public bool TryGetAsInt(string property_name, out int value) {
			string property_value;
			if ( TryGetAsString(property_name, out property_value) ) {
				if ( int.TryParse(property_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ) {
					return true;
				}
			}
			value = 0;
			return false;
		}

		public bool TryGetAsLong(string property_name, out long value) {
			string property_value;
			if ( TryGetAsString(property_name, out property_value) ) {
				if ( long.TryParse(property_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ) {
					return true;
				}
			}
			value = 0;
			return false;
		}

		public bool TryGetAsFloat(string property_name, out float value) {
			string property_value;
			if ( TryGetAsString(property_name, out property_value) ) {
				if ( float.TryParse(property_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ) {
					return true;
				}
			}
			value = 0;
			return false;
		}

		public bool TryGetAsDouble(string property_name, out double value) {
			string property_value;
			if ( TryGetAsString(property_name, out property_value) ) {
				if ( double.TryParse(property_value, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ) {
					return true;
				}
			}
			value = 0;
			return false;
		}

		public bool TryGetAsString(string property_name, out string value) {
			if ( _properties != null ) {
				for ( int i = 0, e = _properties.Count / 2; i < e; ++i ) {
					if ( _properties[i * 2] == property_name ) {
						value = _properties[i * 2 + 1];
						return true;
					}
				}
			}
			value = string.Empty;
			return false;
		}

		// -----------------------------
		// For editor
		// -----------------------------

	#if UNITY_EDITOR
		bool _showProperties = true;
		public void OnInspectorGUI(string label) {
			if ( Count > 0 ) {
				_showProperties = EditorGUILayout.Foldout(_showProperties, label);
				if ( _showProperties ) {
					for ( int i = 0, e = Count; i < e; ++i ) {
						EditorGUILayout.LabelField(GetKeyByIndex(i), GetValueByIndex(i));
					}
				}
			}
		}
	#endif
	}
}
