using Environment = Checkout.Helpers.Environment;

namespace Checkout.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new APIClient("sk_test_32b9cb39-1cd6-4f86-b750-7069a133667d", Environment.Local);

            client.LookupsService.GetBinLookup("foo");
        }
    }
}
