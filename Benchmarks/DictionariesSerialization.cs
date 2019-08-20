using Apex.Collections.Immutable;
using Apex.Serialization;
using BenchmarkDotNet.Attributes;
using Sasa.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace Benchmarks
{
    public class DictionariesSerialization
    {
        private Dictionary<int, int> _dict;
        private ImmutableDictionary<int, int> _immDict;
        private ImmutableSortedDictionary<int, int> _immSortedDict;
        private Trie<int, int> _sasaTrie;
        private HashMap<int, int> _hashMap;

        private MemoryStream _dStream;
        private MemoryStream _idStream;
        private MemoryStream _isdStream;
        private MemoryStream _sasaStream;
        private MemoryStream _hashMapStream;

        private IBinary _serializer;

        public DictionariesSerialization()
        {
            _serializer = Binary.Create(new Settings { SerializationMode = Mode.Graph });
            _dict = new Dictionary<int, int>();
            _immDict = ImmutableDictionary<int, int>.Empty;
            _immSortedDict = ImmutableSortedDictionary<int, int>.Empty;
            _sasaTrie = Trie<int, int>.Empty;
            _hashMap = HashMap<int, int>.Empty;

            for(int i=0;i<10000;++i)
            {
                _dict.Add(i, i);
                _immDict = _immDict.SetItem(i, i);
                _immSortedDict = _immSortedDict.SetItem(i, i);
                _sasaTrie = _sasaTrie.Add(i, i);
                _hashMap = _hashMap.SetItem(i, i);
            }

            _dStream = new MemoryStream();
            _idStream = new MemoryStream();
            _isdStream = new MemoryStream();
            _sasaStream = new MemoryStream();
            _hashMapStream = new MemoryStream();

            _serializer.Write(_dict, _dStream);
            _serializer.Write(_immDict, _idStream);
            _serializer.Write(_immSortedDict, _isdStream);
            _serializer.Write(_sasaTrie, _sasaStream);
            _serializer.Write(_hashMap, _hashMapStream);
        }

        [Benchmark]
        public void SerializeDictionary()
        {
            _dStream.Seek(0, SeekOrigin.Begin);
            _serializer.Write(_dict, _dStream);
        }

        [Benchmark]
        public void SerializeID()
        {
            _idStream.Seek(0, SeekOrigin.Begin);
            _serializer.Write(_immDict, _idStream);
        }

        [Benchmark]
        public void SerializeISD()
        {
            _isdStream.Seek(0, SeekOrigin.Begin);
            _serializer.Write(_immSortedDict, _isdStream);
        }

        [Benchmark]
        public void SerializeSasaTrie()
        {
            _sasaStream.Seek(0, SeekOrigin.Begin);
            _serializer.Write(_sasaTrie, _sasaStream);
        }

        [Benchmark]
        public void SerializeHashMap()
        {
            _hashMapStream.Seek(0, SeekOrigin.Begin);
            _serializer.Write(_hashMap, _hashMapStream);
        }

        [Benchmark]
        public void DeserializeDictionary()
        {
            _dStream.Seek(0, SeekOrigin.Begin);
            _serializer.Read<Dictionary<int, int>>(_dStream);
        }

        [Benchmark]
        public void DeserializeID()
        {
            _idStream.Seek(0, SeekOrigin.Begin);
            _serializer.Read<ImmutableDictionary<int, int>>(_idStream);
        }

        [Benchmark]
        public void DeserializeISD()
        {
            _isdStream.Seek(0, SeekOrigin.Begin);
            _serializer.Read<ImmutableSortedDictionary<int, int>>(_isdStream);
        }

        [Benchmark]
        public void DeserializeSasaTrie()
        {
            _sasaStream.Seek(0, SeekOrigin.Begin);
            _serializer.Read<Trie<int, int>>(_sasaStream);
        }

        [Benchmark]
        public void DeserializeHashMap()
        {
            _hashMapStream.Seek(0, SeekOrigin.Begin);
            _serializer.Read<HashMap<int, int>>(_hashMapStream);
        }
    }
}
