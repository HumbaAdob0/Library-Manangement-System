using Library_Management_System.Data;
using Library_Management_System.Models;

namespace Library_Management_System.Services
{
    /// <summary>
    /// Service for member-related business logic.
    /// </summary>
    public class MemberService
    {
        private readonly IRepository<Member> _memberRepository;

        public MemberService(IRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        public async Task AddMemberAsync(Member member)
        {
            member.JoinDate = DateTime.Now;
            member.IsActive = true;
            await _memberRepository.AddAsync(member);
        }

        public async Task UpdateMemberAsync(Member member)
        {
            await _memberRepository.UpdateAsync(member);
        }

        public async Task DeleteMemberAsync(int id)
        {
            await _memberRepository.DeleteAsync(id);
        }
    }
}
