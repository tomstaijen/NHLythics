using System;
using System.Collections.Generic;
using NHLythics.Model;

namespace NHLythics
{
    public class ModelChecker
    {
        private List<Problem> _problems = new List<Problem>();
        public List<Problem> Problems { get { return _problems; } }

        protected ModelChecker(MappingModel model)
        {
            Model = model;
        }

        public static ModelChecker Build(Action<ModelBuilder> config)
        {
            var model = new MappingModel();
            var checker = new ModelChecker(model);
            var builder = new ModelBuilder(checker);

            config(builder);

            return checker;
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
