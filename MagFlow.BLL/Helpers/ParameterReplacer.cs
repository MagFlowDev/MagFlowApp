using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MagFlow.BLL.Helpers
{
    public class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _target;
        private readonly ParameterExpression _replacement;

        public ParameterReplacer(ParameterExpression target, ParameterExpression replacement)
        {
            _target = target;
            _replacement = replacement;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _target ? _replacement : base.VisitParameter(node);
        }
    }
}
