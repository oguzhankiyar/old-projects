using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.ModelEntities;
using OK.Mobisis.Api.Helper.Parsings;

namespace OK.Mobisis.Api.Helper.Requests
{
    public static class DocumentRequests
    {
        public static BaseRequest<List<Document>> DocumentRequest;

        static DocumentRequests()
        {
            DocumentRequest = BaseRequest<List<Document>>.GetInstance();
        }

        public static void GetDocuments(string studentId, string password)
        {
            var requestObj = DocumentParsings.GetDocuments(studentId, password);
            DocumentRequest.OnPopulated = (jsonResult) =>
            {
                DocumentRequest.Response = DocumentParsings.ParseDocuments(jsonResult);
            };
            Global.OnRequestCalled("DocumentRequests.GetDocuments", new object[] { studentId, password });
            DocumentRequest.GetResult(requestObj);
        }
    }
}
