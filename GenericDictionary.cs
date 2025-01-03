﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Generic Serializable Dictionary for Unity 2020.1 and above.
/// Simply declare your key/value types and you're good to go - zero boilerplate.
/// </summary>
[Serializable]
public class GenericDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver {
    [SerializeField] private List<KeyValuePair> list = new();

    private Dictionary<TKey, int> _indexByKey = new();
    private Dictionary<TKey, TValue> _dict = new();

#pragma warning disable 0414
    [SerializeField, HideInInspector] private bool keyCollision;
#pragma warning restore 0414

    [Serializable]
    private struct KeyValuePair {
        public TKey key;
        public TValue value;

        public KeyValuePair(TKey key, TValue value) {
            this.key = key;
            this.value = value;
        }
    }

    // Lists are serialized natively by Unity, no custom implementation needed.
    public void OnBeforeSerialize() {
    }

    // Populate dictionary with pairs from list and flag key-collisions.
    public void OnAfterDeserialize() {
        _dict.Clear();
        _indexByKey.Clear();
        keyCollision = false;
        for (var i = 0; i < list.Count; i++) {
            var key = list[i].key;
            if (key != null && !ContainsKey(key)) {
                _dict.Add(key, list[i].value);
                _indexByKey.Add(key, i);
            }
            else {
                keyCollision = true;
            }
        }
    }

    // IDictionary
    public TValue this[TKey key] {
        get => _dict[key];
        set {
            _dict[key] = value;
            if (_indexByKey.ContainsKey(key)) {
                var index = _indexByKey[key];
                list[index] = new KeyValuePair(key, value);
            }
            else {
                list.Add(new KeyValuePair(key, value));
                _indexByKey.Add(key, list.Count - 1);
            }
        }
    }

    public ICollection<TKey> Keys => _dict.Keys;
    public ICollection<TValue> Values => _dict.Values;

    public void Add(TKey key, TValue value) {
        _dict.Add(key, value);
        list.Add(new KeyValuePair(key, value));
        _indexByKey.Add(key, list.Count - 1);
    }

    public bool ContainsKey(TKey key) {
        return _dict.ContainsKey(key);
    }

    public bool Remove(TKey key) {
        if (_dict.Remove(key)) {
            var index = _indexByKey[key];
            list.RemoveAt(index);
            UpdateIndexLookup(index);
            _indexByKey.Remove(key);
            return true;
        }
        return false;
    }

    private void UpdateIndexLookup(int removedIndex) {
        for (var i = removedIndex; i < list.Count; i++) {
            var key = list[i].key;
            _indexByKey[key]--;
        }
    }

    public bool TryGetValue(TKey key, out TValue value) => _dict.TryGetValue(key, out value);

    // ICollection
    public int Count => _dict.Count;
    public bool IsReadOnly { get; set; }

    public void Add(KeyValuePair<TKey, TValue> pair) {
        Add(pair.Key, pair.Value);
    }

    public void Clear() {
        _dict.Clear();
        list.Clear();
        _indexByKey.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> pair) {
        if (_dict.TryGetValue(pair.Key, out var value)) {
            return EqualityComparer<TValue>.Default.Equals(value, pair.Value);
        }
        return false;
    }

    /// <summary>
    ///  Copy to Array
    /// </summary>
    /// <param name="array">the created array</param>
    /// <param name="arrayIndex">starting at</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
        if (array == null)
            throw new ArgumentException("The array cannot be null.");
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(paramName:arrayIndex.ToString(), "The starting array index cannot be negative.");
        if (array.Length - arrayIndex < _dict.Count)
            throw new ArgumentException("The destination array has fewer elements than the collection.");

        foreach (var pair in _dict) {
            array[arrayIndex] = pair;
            arrayIndex++;
        }
    }

    public bool Remove(KeyValuePair<TKey, TValue> pair) {
        if (_dict.TryGetValue(pair.Key, out var value)) {
            var valueMatch = EqualityComparer<TValue>.Default.Equals(value, pair.Value);
            if (valueMatch) {
                return Remove(pair.Key);
            }
        }

        return false;
    }

    // IEnumerable
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dict.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();
}