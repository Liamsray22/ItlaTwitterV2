using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailConfig
{
    public interface IMessage
    {
        Task SendMailAsync(Message message);
    }
}
