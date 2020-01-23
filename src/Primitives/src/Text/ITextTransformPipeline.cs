
namespace NerdyMishka.Text
{
    public interface ITextTransformPipeline : ITextTransform
    {
        void Add(ITextTransform transformer);
    }
}