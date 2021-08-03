using Microsoft.Xna.Framework.Graphics;

namespace Pictomancer.Graphics
{
    public class GraphicsDeviceState
    {
        public BlendState BlendState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public SamplerState SamplerState { get; set; }

        public void Restore(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.BlendState = BlendState;
            graphicsDevice.DepthStencilState = DepthStencilState;
            graphicsDevice.RasterizerState = RasterizerState;
            graphicsDevice.SamplerStates[0] = SamplerState;
        }
    }
}