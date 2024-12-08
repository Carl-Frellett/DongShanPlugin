//[RichTextTag]
//public class NobrTag : NoParamsTag
//{
//    /// <inheritdoc/>
//    public override string[] Names { get; } = { "nobr" };

//    /// <inheritdoc/>
//    public override bool HandleTag(ParserContext context)
//    {
//        return false;

//        context.NoBreak = true;

//        context.AddEndingTag<CloseNobrTag>();
//        context.ResultBuilder.Append("<nobr>");

//        return true;
//    }
//}
