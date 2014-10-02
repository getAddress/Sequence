using System;


namespace getAddress.Sequence.Azure
{
    public abstract class AzureTableException : Exception
    {
        protected AzureTableException(string message, int httpResult)
            : base(message)
        {
            HttpResult = httpResult;
        }

        public int HttpResult
        {
            get;
            private set;
        }


    }

    public class AzureTableAddException : AzureTableException
    {
        public AzureTableAddException(string message, int httpResult)
            : base(message, httpResult)
        {

        }

    }


    public class AzureTableNameException : Exception
    {
        public AzureTableNameException(string tableName, string message)
            : base(string.Format(message, tableName))
        {

        }


    }

    public class AzureContainerNameException : Exception
    {
        public AzureContainerNameException(string continerName, string message)
            : base(string.Format(message, continerName))
        {

        }


    }
    public class AzureBlobNameException : Exception
    {
        public AzureBlobNameException(string blobName, string message)
            : base(string.Format(message, blobName))
        {

        }


    }



    public class AzureTableUpdateException : AzureTableException
    {
        public AzureTableUpdateException(string message, int httpResult)
            : base(message, httpResult)
        {

        }


    }

    

    public class AzureTableDeleteException : AzureTableException
    {
        public AzureTableDeleteException(string message, int httpResult)
            : base(message, httpResult)
        {

        }


    }

    public class AzureTableLookUpException : AzureTableException
    {
        public AzureTableLookUpException(string message, int httpResult)
            : base(message, httpResult)
        {

        }


    }
}
