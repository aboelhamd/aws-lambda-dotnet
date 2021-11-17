using System;
using System.Linq;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

namespace TestServerlessApp
{
    public class Foobar
    {
        private readonly Greeter greeter;

        public Foobar()
        {
            greeter = new Greeter();
        }

        public Amazon.Lambda.APIGatewayEvents.APIGatewayProxyResponse SayHello(Amazon.Lambda.APIGatewayEvents.APIGatewayProxyRequest request, Amazon.Lambda.Core.ILambdaContext context)
        {
            var firstNames = default(System.Collections.Generic.IEnumerable<string>);
            if (request.MultiValueQueryStringParameters?.ContainsKey("names") == true)
            {
                try
                {
                    firstNames = request.MultiValueQueryStringParameters["names"].Select(q => (string) Convert.ChangeType(q, typeof(string))).ToList();
                }
                catch (Exception e) when (e is InvalidCastException || e is FormatException || e is OverflowException || e is ArgumentException)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 400
                    };
                }
            }

            greeter.SayHello(firstNames, request, context);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200
            };
        }
    }
}