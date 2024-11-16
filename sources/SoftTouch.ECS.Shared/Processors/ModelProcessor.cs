using SoftTouch.ECS.Processors;
using SoftTouch.ECS.Querying;
using SoftTouch.ECS.Shared.Components;
using System.Linq;
using System.Numerics;

namespace SoftTouch.ECS.Shared.Processors
{
    public class ModelProcessor : Processor<Query<ModelComponent, NoFilter>>
    {
        public ModelProcessor(World world) : base(world)
        {
        }

        public override void Update()
        {
            // foreach((var e, var model) in query1)
            // {
                
            // }
        }
    }
}