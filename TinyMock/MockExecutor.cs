using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyMock
{
    public class MockExecutor
    {
        private const string Prefix = "element";
        private List<MockModel> _mockSet;
        private MockModel _model;
        public MockExecutor(MockModel model)
        {
            _model = model;
            _mockSet = Tdb.MockSet;
        }

        private List<MockModel> Create()
        {
            _model.Id = GenerateCounterId();
            var result = new List<MockModel>(_mockSet) { _model };
            Tdb.UpdateSet(result);
            return result;
        }

        private List<MockModel> Update()
        {
            var result = new List<MockModel>();
            foreach (var mock in _mockSet)
            {
                if (mock.Id == _model.Id)
                {
                    try
                    {
                        if ((_model.Scheme as dynamic).type != null)
                        {
                        }
                    }
                    catch (Exception)
                    {
                        (_model.Scheme as dynamic).type = (mock.Scheme as dynamic).type;
                    }
                    result.Add(_model);
                }
                else
                {
                    result.Add(mock);
                }
            }
            Tdb.UpdateSet(result);
            return result;
        }

        private List<MockModel> Delete(string id)
        {
            var result = new List<MockModel>();
            foreach (var mock in _mockSet)
            {
                if (mock.Id == id)
                {
                    continue;
                }
                result.Add(mock);
            }
            Tdb.UpdateSet(result);
            return result;
        }

        private List<MockModel> Clear()
        {
            Tdb.UpdateSet(null);
            return Tdb.MockSet;
        }

        private string GenerateCounterId()
        {
            var lastId = _mockSet.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return Prefix + "." + 1;
            }
            var counter = int.Parse(lastId.Split('.').Last());
            counter++;
            return Prefix + "." + counter;
        }

        public List<MockModel> PerformAction()
        {
            switch (_model.Action)
            {
                case ActionType.Create:
                    return Create();
                case ActionType.Edit:
                    return Update();
                case ActionType.Delete:
                    return Delete(_model.Id);
                case ActionType.Clear:
                    return Clear();
                case ActionType.Undefined:
                    return _mockSet;
                default: return _mockSet;
            }
        }
    }
}