using System.Text;

namespace ArkDataProcessor
{
    class StoreTsvDataPipelineTask : DataProcessingPipelineTaskNoResult<IEnumerable<object[]>, string>
    {
        internal override Task Execute(IEnumerable<object[]> arg1, string arg2)
        {
            using(var writer = new StreamWriter(arg2))
            {
                foreach (var record in arg1)
                {
                    var line = new StringBuilder();

                    line.Append(record[0]);

                    for (int i = 1; i < record.Length; i++)
                    {
                        var item = record[i];
                        line.Append('\t');
                        line.Append(item);
                    }

                    writer.WriteLine(line);
                }
            }
            return Task.CompletedTask;
        }
    }
}
