using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//namespace UnityEngine.Experiemntal.Rendering.Universal
//{

    public class HLRenderPass : ScriptableRenderPass
    {
        public Material mMat;
        //使用第几个pass
        public int blitShaderPassIndex = 0;
        public FilterMode filterMode { get; set; }
        private RenderTargetIdentifier source { get; set; }
        private RenderTargetHandle destination { get; set; }

        RenderTargetHandle m_temporaryColorTexture;

        string m_ProfilerTag;//专门给profiler看的名字
        public HLRenderPass(string passname, RenderPassEvent _event, Material _mat,float contrast)
        {
            m_ProfilerTag = passname;
            this.renderPassEvent = _event;
            mMat = _mat;
            mMat.SetFloat("_Contrast", contrast);
            m_temporaryColorTexture.Init("temporaryColorTexture");
        }
        public void Setup(RenderTargetIdentifier src, RenderTargetHandle dest)
        {
            this.source = src;
            this.destination = dest;
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;
            //不能读写同一个颜色target，创建一个临时的render Target去blit
            if (destination == RenderTargetHandle.CameraTarget)
            {
                cmd.GetTemporaryRT(m_temporaryColorTexture.id, opaqueDesc, filterMode);
                Blit(cmd, source, m_temporaryColorTexture.Identifier(), mMat, blitShaderPassIndex);
                Blit(cmd, m_temporaryColorTexture.Identifier(), source);
            }
            else
            {
                Blit(cmd, source, destination.Identifier(), mMat, blitShaderPassIndex);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (destination == RenderTargetHandle.CameraTarget)
                cmd.ReleaseTemporaryRT(m_temporaryColorTexture.id);
        }
    }
//}
