using Silownia.DAL;
using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Silownia
{
    public static class KryteriaSzukania
    {

        public static IQueryable<T> Search<T>(this IQueryable<T> source,
                                         string searchTerm,
                                         params Expression<Func<T, string>>[] stringProperties)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            var searchTermExpression = Expression.Constant(searchTerm);

            //Variable to hold merged 'OR' expression
            Expression orExpression = null;
            //Retrieve first parameter to use accross all expressions
            var singleParameter = stringProperties[0].Parameters.Single();

            //Build a contains expression for each property
            foreach (var stringProperty in stringProperties)
            {
                //Syncronise single parameter accross each property
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty, stringProperty.Parameters.Single(), singleParameter);

                //Build expression to represent x.[propertyX].Contains(searchTerm)
                var containsExpression = BuildContainsExpression(swappedParamExpression, searchTermExpression);

                orExpression = BuildOrExpression(orExpression, containsExpression);
            }

            var completeExpression = Expression.Lambda<Func<T, bool>>(orExpression, singleParameter);
            return source.Where(completeExpression);
        }

        private static Expression BuildOrExpression(Expression existingExpression, Expression expressionToAdd)
        {
            if (existingExpression == null)
            {
                return expressionToAdd;
            }

            //Build 'OR' expression for each property
            return Expression.OrElse(existingExpression, expressionToAdd);
        }

        private static MethodCallExpression BuildContainsExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression)
        {
            return Expression.Call(stringProperty.Body, typeof(string).GetMethod("Contains"), searchTermExpression);
        }

        //Create SwapVisitor to merge the parameters from each property expression into one
        public class SwapVisitor : ExpressionVisitor
        {
            private readonly Expression from, to;
            public SwapVisitor(Expression from, Expression to)
            {
                this.from = from;
                this.to = to;
            }
            public override Expression Visit(Expression node)
            {
                return node == from ? to : base.Visit(node);
            }
            public static Expression Swap(Expression body, Expression from, Expression to)
            {
                return new SwapVisitor(from, to).Visit(body);
            }
        }


        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
         (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        //public static IQueryable imieNazwisko(IQueryable kolekcja, string szukana)
        //{

        //    if (!String.IsNullOrEmpty(szukana))
        //    {
        //        kolekcja = kolekcja.Cast<Osoba>().Where(s => s.Imie.Contains(szukana) || s.Nazwisko.Contains(szukana));
        //    }
        //    return (IQueryable)kolekcja;
        //}

        //public static IQueryable miasto(IQueryable kolekcja, string szukana)
        //{

        //    if (!String.IsNullOrEmpty(szukana))
        //    {
        //        kolekcja = kolekcja.Cast<Osoba>().Where(s => s.Adres.Miasto.Contains(szukana));
        //    }
        //    return kolekcja;
        //}

        //public static IQueryable<Klient> umowa(IQueryable<Klient> kolekcja, bool umowa)
        //{

        //    if (umowa)
        //    {
        //        kolekcja = kolekcja.Cast<Klient>().Where(s => s.Umowy.Count > 0);
        //    }
        //    return (IQueryable<Klient>)kolekcja;
        //}
    }
}