using System;
using System.Collections.Generic;
using NHLythics.Model;

namespace NHLythics
{
    public class ModelChecker
    {
        private List<Problem> _problems = new List<Problem>();
        public List<Problem> Problems { get { return _problems; } }

        private List<Action<ModelChecker>> _actions = new List<Action<ModelChecker>>();
        protected ModelChecker(MappingModel model, ICollection<Action<ModelChecker>> actions)
        {
            Model = model;
            _actions.AddRange(actions);
        }

        public static ModelChecker Build(Action<ModelBuilder> config)
        {
            var model = new MappingModel();
            var builder = new ModelBuilder();

            config(builder);

            return new ModelChecker(model, builder.Actions);
        }

        public MappingModel Model { get; private set; }

        private List<IModelCheckerHandler> _observers = new List<IModelCheckerHandler>();

        public void Apply(ModelCheckerModuleBase module)
        {
            module.ModelChecker = this;
            module.Run();
        }

        public void RegisterObserver(IModelCheckerHandler handler)
        {
            _observers.Add(handler);
        }

        public void RegisterProblem(Problem problem)
        {
            _observers.ForEach(o => o.Notify(problem));
        }

        public void Validate()
        {
            foreach (var a in _actions)
            {
                a(this);
            }
        }
    }

    public abstract class ModelCheckerModuleBase
    {
        public ModelChecker ModelChecker { get; set; }
        public MappingModel Model { get { return ModelChecker.Model; } }

        public void RegisterProblem(Problem problem)
        {
            ModelChecker.RegisterProblem(problem);
        }

        public abstract void Run();
    }

    /// <summary>
    /// Base to only process each entity
    /// </summary>
    public abstract class EntityCheckerBase : ModelCheckerModuleBase
    {
        public override void Run()
        {
            foreach (var entity in ModelChecker.Model.Entities.Values)
            {
                Check(entity);
            }
        }

        public abstract void Check(Entity entity);
    }
}
