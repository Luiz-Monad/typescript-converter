﻿using System.Collections.Generic;

/*
 * Copyright (c) 2011, 2017, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

namespace com.sun.source.tree
{

    /// <summary>
    /// A tree node for a lambda expression.
    /// 
    /// For example:
    /// <pre>{@code
    ///   ()->{}
    ///   (List<String> ls)->ls.size()
    ///   (x,y)-> { return x + y; }
    /// }</pre>
    /// </summary>
    public interface LambdaExpressionTree : ExpressionTree
    {
        /// <summary>
        /// Returns the parameters of this lambda expression. </summary>
        /// <returns> the parameters </returns>
        //JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Java wildcard generics have no direct equivalent in .NET:
        //ORIGINAL LINE: java.util.List<? extends VariableTree> getParameters();
        IList<VariableTree> getParameters();

        /// <summary>
        /// Returns the body of the lambda expression. </summary>
        /// <returns> the body </returns>
        Tree getBody();

        /// <summary>
        /// Returns the kind of the body of the lambda expression. </summary>
        /// <returns> the kind of the body </returns>
        BodyKind getBodyKind();
    }

    /// <summary>
    /// Lambda expressions come in two forms:
    /// <ul>
    /// <li> expression lambdas, whose body is an expression, and
    /// <li> statement lambdas, whose body is a block
    /// </ul>
    /// </summary>
    public enum BodyKind
    {
        /// <summary>
        /// enum constant for expression lambdas </summary>
        EXPRESSION,
        /// <summary>
        /// enum constant for statement lambdas </summary>
        STATEMENT
    }

}