using System;

public interface IRequestQueue
{
    void Enqueue(IRequest request);
    void Cancel(Type requestType);          
    void CancelActive(Type requestType);   
}